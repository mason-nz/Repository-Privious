package com.xyauto.service;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import lombok.extern.slf4j.Slf4j;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.xyauto.domain.AEAutocompleter;
import com.xyauto.domain.AreaInfo;
import com.xyauto.domain.DictInfo;
import com.xyauto.domain.RoleInfo;
import com.xyauto.domain.UserInfo;
import com.xyauto.domain.UserInfoOrder;
import com.xyauto.domain.UserInfoRequest;
import com.xyauto.mapper.AreaInfoMapper;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.mapper.RoleInfoMapper;
import com.xyauto.mapper.UserInfoMapper;
import com.xyauto.mapper.UserInfoRequestMapper;
import com.xyauto.util.CacheUtil;
import com.xyauto.util.DesUtil;
import com.xyauto.util.LogInfoUtil;
import com.xyauto.util.Md5Utils;
import com.xyauto.util.SMSUtil;
import com.xyauto.util.StringUtil;
import com.xyauto.util.SysConstants;

@Service
@Slf4j
public class UserInfoService {
	@Autowired
	private UserInfoMapper userInfoMapper;
	@Autowired
	private RoleInfoMapper roleInfoMapper;
	@Autowired
	private UserInfoRequestMapper userInfoRequestMapper;
	@Autowired
	private LogInfoUtil logInfo;
	@Autowired
	private AreaInfoMapper areaMapper;
	@Autowired
	private DictInfoMapper dictMapper;
	@Value("${com.xyauto.service.defaultPassword}")
	private String defaultPassword;

	public List<UserInfoRequest> checkMobileRegister(UserInfoRequest userInfo) {
		return userInfoRequestMapper.checkMobileRegister(userInfo);
	}

	/**
	 * 注册
	 * 
	 * @param userInfo
	 */
	@Transactional
	public void insertUserInfo(UserInfoRequest userInfo, UserInfo loginUser) {
		String message = "注册成功";
		userInfo.setUserName(userInfo.getMobile());
		String pwd = Md5Utils.getMD5(userInfo.getPwd() + userInfo.getCategory()
				+ SysConstants.LOGINPWDKEY, SysConstants.MD5CHARTSET);
		userInfo.setPwd(pwd);
		userInfo.setIsAuthAE(false);
		userInfo.setIsAuthMTZ(false);
		userInfo.setStatus(SysConstants.STATUS_0);
		userInfo.setSource(SysConstants.SOURCE_2);
		userInfo.setCreateTime(new Date());
		userInfo.setLastUpdateTime(new Date());
		userInfoRequestMapper.registerUserInfo(userInfo);
		// 更新用户信息
		userInfo.setLastUpdateTime(new Date());
		Integer userID = userInfo.getUserID();
		userInfo.setCreateUserID(userID);
		userInfo.setLastUpdateUserID(userID);
		userInfo.setSysID(SysConstants.SYS_ID);
		userInfoRequestMapper.updateUserInfo(userInfo);
		log.debug(UserInfoService.class.getName() + "----insertUserInfo:"
				+ userInfo);
		// 增加角色表
		userInfo.setCreateTime(new Date());
		Integer category = userInfo.getCategory();
		if (category == 29001) {
			userInfo.setRoleID(SysConstants.ROLE_ADD);
		}
		if (category == 29002) {
			userInfo.setRoleID(SysConstants.ROLE_MEDIA);
		}
		userInfoRequestMapper.registerUserRole(userInfo);
		loginUser.setUserID(userID);
		loginUser.setUserName(userInfo.getUserName());
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.INSERT,
				SysConstants.USER_ACCOUNT_MANAGER);
	}

	public Map<String, Object> getUserInfoReq(UserInfoRequest userInfoRequest,
			UserInfo loginUser) {
		String message = "查看用户信息";
		Map<String, Object> map = new HashMap<>();
		userInfoRequest.setPageRows(SysConstants.PAGEROWS);
		userInfoRequest.setCount(-1);
		Calendar cal = Calendar.getInstance();
		Date date = null;
		if (null != userInfoRequest.getCreateEndTime()
				&& !userInfoRequest.getCreateEndTime().trim().equals("")) {
			try {
				date = (new SimpleDateFormat("yyyy-MM-dd"))
						.parse(userInfoRequest.getCreateEndTime());
			} catch (ParseException e) {
				e.printStackTrace();
				log.error("Error Occured" + e);
				return map;
			}
			cal.setTime(date);
			cal.add(Calendar.DATE, 1);
			userInfoRequest.setCreateEndTime(new SimpleDateFormat("yyyy-MM-dd")
					.format(cal.getTime()));
		}
		if (null != userInfoRequest) {
			if (null != userInfoRequest.getAeName()) {
				userInfoRequest.setAeName(StringUtil.removeTrim(userInfoRequest
						.getAeName()));
			}
			if (null != userInfoRequest.getMobile()) {
				userInfoRequest.setMobile(StringUtil.removeTrim(userInfoRequest
						.getMobile()));
			}
			if (null != userInfoRequest.getTrueName()) {
				userInfoRequest.setTrueName(StringUtil
						.removeTrim(userInfoRequest.getTrueName()));
			}
			if (null != userInfoRequest.getCreateStartTime()) {
				userInfoRequest.setCreateStartTime(StringUtil
						.removeTrim(userInfoRequest.getCreateStartTime()));
			}
			if (null != userInfoRequest.getCreateEndTime()) {
				userInfoRequest.setCreateEndTime(StringUtil
						.removeTrim(userInfoRequest.getCreateEndTime()));
			}

		}
		List<UserInfoRequest> userList = userInfoRequestMapper
				.getUserInfoReq(userInfoRequest);
		userList = getUser(userList);
		if (userList != null && userList.size() != 0) {
			userList.get(0).setCount(userInfoRequest.getCount());
			userList.get(0).setPageRows(SysConstants.PAGEROWS);
		}
		List<AEAutocompleter> aeRole = userInfoRequestMapper
				.getAEList(SysConstants.ROLE_AE);
		Map<Integer, DictInfo> dictMap = CacheUtil.getDictMap(dictMapper);
		Collection<DictInfo> dictValues = dictMap.values();
		map.put("dictValues", dictValues);
		map.put("userInfo", userList);
		map.put("aeRole", aeRole);
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.SELECT,
				SysConstants.USER_ACCOUNT_MANAGER);
		return map;
	}

	/**
	 * 处理列表数据
	 * 
	 * @param userInfoReq
	 * @return
	 */
	public List<UserInfoRequest> getUser(List<UserInfoRequest> userInfoReq) {
		if (null != userInfoReq && userInfoReq.size() != 0) {
			Map<Integer, AreaInfo> areaMap = CacheUtil.getAreaMap(areaMapper);
			Map<Integer, DictInfo> dictMap = CacheUtil.getDictMap(dictMapper);
			SimpleDateFormat formatter = new SimpleDateFormat(
					"yyyy-MM-dd HH:mm");
			for (UserInfoRequest userInfoRequest : userInfoReq) {
				try {
					DesUtil des = new DesUtil();
					userInfoRequest.setUserIDPwd(URLEncoder.encode(
							des.encode(userInfoRequest.getUserID().toString()),
							"UTF-8"));
				} catch (UnsupportedEncodingException e) {
					e.printStackTrace();
					log.error("Error Occured" + e);
				} catch (Exception e) {
					e.printStackTrace();
					log.error("Error Occured" + e);
				}
				String format = formatter.format(userInfoRequest
						.getCreateTime());
				userInfoRequest.setCreateStartTime(format);
				String address = "";
				if (null != userInfoRequest.getProvinceID()
						&& userInfoRequest.getProvinceID() > -1) {
					address += ""
							+ areaMap.get(userInfoRequest.getProvinceID())
									.getAreaName();
				}
				if (null != userInfoRequest.getCityID()
						&& userInfoRequest.getCityID() > -1) {
					address += ""
							+ areaMap.get(userInfoRequest.getCityID())
									.getAreaName();
				}
				if (null != userInfoRequest.getCounntyID()
						&& userInfoRequest.getCounntyID() > -1) {
					address += ""
							+ areaMap.get(userInfoRequest.getCounntyID())
									.getAreaName();
				}
				userInfoRequest.setAddress(address);
				DictInfo source = dictMap.get(userInfoRequest.getSource());
				userInfoRequest.setSourceName(source.getDictName());
				Integer status = userInfoRequest.getStatus();
				if (status == SysConstants.STATUS_0) {
					userInfoRequest.setStatusName("启用");
				}
				if (status == SysConstants.STATUS_1) {
					userInfoRequest.setStatusName("禁用");
				}
				if (null != userInfoRequest.getType()) {
					DictInfo type = dictMap.get(userInfoRequest.getType());
					userInfoRequest.setTypeName(type.getDictName());
				}
				DictInfo business = dictMap
						.get(userInfoRequest.getBusinessID());
				if (null != business) {
					userInfoRequest.setBusinessName(business.getDictName());
				}
			}
		}
		return userInfoReq;
	}

	@Transactional
	public void upUserInfoByRquest(UserInfoRequest[] array, UserInfo loginUser) {
		String message = "";
		String pwd = "";
		boolean flag = false;
		List<UserInfoRequest> userList = new ArrayList<>();
		for (UserInfoRequest req : array) {
			req.setLastUpdateTime(new Date());
			req.setLastUpdateUserID(loginUser.getUserID());
			// 初始化密码
			if (null != req.getMark() && req.getMark() == 1) {
				pwd = Md5Utils.getMD5(defaultPassword + req.getCategory()
						+ SysConstants.LOGINPWDKEY, SysConstants.MD5CHARTSET);
				req.setPwd(pwd);
				message = "重置用户密码";
				flag = true;
			}
			if (req.getStatus() != null) {
				message = "修改用户状态";
				flag = false;
			}
			if (req.getAuthAEUserID() != null) {
				message = "给用户指定AE";
				flag = false;
			}
			userList.add(req);

			log.debug("---upUserInfoByRquest lastUpdateTime:" + req);
		}
		int resultStatus = userInfoRequestMapper.upUserInfoByRquest(userList);
		for (UserInfoRequest req : array) {
			if (resultStatus > 0 && null != req.getMark() && req.getMark() == 1
					&& flag) {
				SMSUtil.sendSMS(req.getMobile(),
						SysConstants.RESET_PASS_MESSAGE.replace("X",
								defaultPassword));
				log.debug("---mobile:"
						+ req.getMobile()
						+ "--sms_message:"
						+ SysConstants.RESET_PASS_MESSAGE.replace("X",
								defaultPassword));
			}
		}

		logInfo.insertUserLogInfo(loginUser, message, SysConstants.UPDATE,
				SysConstants.USER_ACCOUNT_MANAGER);
	}

	public List<UserInfoRequest> getAERole(UserInfoRequest userInfoRequest,
			UserInfo loginUser) {
		String message = "获取全部AE角色信息";
		userInfoRequest.setRoleID(SysConstants.ROLE_AE);
		log.debug("---" + userInfoRequest);
		List<UserInfoRequest> userList = userInfoRequestMapper
				.getAERole(userInfoRequest);
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.SELECT,
				SysConstants.USER_ACCOUNT_MANAGER);
		return userList;
	}

	@Transactional
	public void updateUserAuthAE(UserInfoRequest[] array, UserInfo loginUser) {
		String message = "修改用户的授权AE信息";
		List<UserInfoRequest> userList = new ArrayList<>();
		for (UserInfoRequest userInfoRequest : array) {
			userInfoRequest.setLastUpdateTime(new Date());
			userInfoRequest.setLastUpdateUserID(loginUser.getUserID());
			userList.add(userInfoRequest);
		}
		userInfoRequestMapper.updateUserAuthAE(userList);
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.UPDATE,
				SysConstants.USER_ROLE_MANAGER);
	}

	public Map<String, Object> getUserOperationList(
			UserInfoRequest userInfoRequest, UserInfo loginUser) {
		String message = "列表查询";
		Map<String, Object> userMap = new HashMap<>();
		// 查询所有角色
		List<RoleInfo> roleList = roleInfoMapper.selectAll();
		// 查询用户列表
		userInfoRequest.setPageRows(SysConstants.PAGEROWS);
		userInfoRequest.setCount(-1);
		List<UserInfoRequest> userList = userInfoRequestMapper
				.getUserOperationList(userInfoRequest);
		if (userList != null && userList.size() != 0) {
			userList.get(0).setCount(userInfoRequest.getCount());
			userList.get(0).setPageRows(SysConstants.PAGEROWS);
		}
		userMap.put("roleList", roleList);
		userMap.put("userInfo", userList);
		SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm");
		if (null != userList) {
			for (UserInfoRequest userInfoRq : userList) {
				if (null != userInfoRq.getCreateTime()) {
					String format = formatter
							.format(userInfoRq.getCreateTime());
					userInfoRq.setCreateStartTime(format);
				}
			}
		}
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.SELECT,
				SysConstants.USER_ACCOUNT_MANAGER);
		return userMap;
	}

	/**
	 * 用户管理-广告主 条件查询
	 * 
	 * @param userInfoRequest
	 * @return
	 */
	public Map<String, Object> selectAddUserList(
			UserInfoRequest userInfoRequest, UserInfo loginUser) {
		String message = "用户列表查询";
		Map<String, Object> userMap = new HashMap<>();
		List<RoleInfo> roleList = roleInfoMapper.selectAll();
		userInfoRequest.setPageRows(SysConstants.PAGEROWS);
		userInfoRequest.setCount(-1);
		if (null != userInfoRequest) {
			if (null != userInfoRequest.getMobile()) {
				userInfoRequest.setMobile(StringUtil.removeTrim(userInfoRequest
						.getMobile()));
			}
			if (null != userInfoRequest.getTrueName()) {
				userInfoRequest.setTrueName(StringUtil
						.removeTrim(userInfoRequest.getTrueName()));
			}
			if (null != userInfoRequest.getRoleID()) {
				userInfoRequest.setRoleID(StringUtil.removeTrim(userInfoRequest
						.getRoleID()));
			}

		}
		List<UserInfoRequest> userList = userInfoRequestMapper
				.selectAddUserList(userInfoRequest);
		if (userList != null && userList.size() != 0) {
			userList.get(0).setCount(userInfoRequest.getCount());
			userList.get(0).setPageRows(SysConstants.PAGEROWS);
		}
		userMap.put("roleList", roleList);
		userMap.put("userInfo", userList);
		SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm");
		if (null != userList) {
			for (UserInfoRequest userInfoRq : userList) {
				String format = formatter.format(userInfoRq.getCreateTime());
				userInfoRq.setCreateStartTime(format);
			}
		}
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.SELECT,
				SysConstants.USER_ACCOUNT_MANAGER);
		return userMap;
	}

	public List<UserInfoOrder> getUserInfoOrder(UserInfoOrder userInfoOrder,
			UserInfo loginUser) {
		String message = "媒体用户订单列表查询";
		String strWhere = "";
		if (userInfoOrder.getCreateUserID() == -1) {
			strWhere = "";
		} else if (loginUser.getRoleIDs().equals(SysConstants.ROLE_MEDIA)) {
			String strSelectMediaID = " (s.MediaID in (select MediaID from Media_Weixin where CreateUserID="
					+ userInfoOrder.getCreateUserID()
					+ ") and  s.MediaType=14001) or (s.MediaID in (select MediaID from Media_PCAPP where CreateUserID="
					+ userInfoOrder.getCreateUserID()
					+ ") and  s.MediaType=14002) or (s.MediaID in (select MediaID from Media_Weibo where CreateUserID="
					+ userInfoOrder.getCreateUserID()
					+ ") "
					+ "and  S.MediaType=14003) or (S.MediaID in (select MediaID from Media_Video where CreateUserID="
					+ userInfoOrder.getCreateUserID()
					+ ") and  S.MediaType=14004) or (S.MediaID in (select MediaID from Media_Broadcast where CreateUserID="
					+ userInfoOrder.getCreateUserID()
					+ ")"
					+ " and s.MediaType=14005)";

			strWhere = " AND (s.CreateUserID="
					+ userInfoOrder.getCreateUserID() + " or "
					+ strSelectMediaID + ")";
		} else {
			strWhere = " and 1=2";
		}
		log.debug("---mediuOrder sql " + strWhere);
		List<UserInfoOrder> userOrderList = userInfoRequestMapper
				.getUserInfoOrder(strWhere);
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.SELECT,
				SysConstants.USER_ACCOUNT_MANAGER);
		return userOrderList;
	}

	/**
	 * 找回密码-修改用户密码
	 * 
	 * @param userInfo
	 * @param loginUser
	 */
	@Transactional
	public void updateUserPWD(UserInfoRequest userInfo, UserInfo loginUser) {
		String message = "找回用户密码";
		String pwd = "";
		pwd = Md5Utils.getMD5(userInfo.getPwd() + userInfo.getCategory()
				+ SysConstants.LOGINPWDKEY, SysConstants.MD5CHARTSET);
		userInfo.setPwd(pwd);
		userInfo.setLastUpdateTime(new Date());
		userInfo.setLastUpdateUserID(userInfo.getUserID());
		userInfoRequestMapper.updateUserPWD(userInfo);
		loginUser.setUserID(userInfo.getUserID());
		loginUser.setUserName(userInfo.getMobile());
		logInfo.insertUserLogInfo(loginUser, message, SysConstants.UPDATE,
				SysConstants.USER_ACCOUNT_MANAGER);
	}

}
