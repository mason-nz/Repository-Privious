package com.xyauto.service;

import java.io.File;
import java.util.Date;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.xyauto.domain.UploadFileInfo;
import com.xyauto.domain.UserDetailInfo;
import com.xyauto.domain.UserExt;
import com.xyauto.domain.UserInfo;
import com.xyauto.domain.UserRole;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.mapper.UploadFileInfoMapper;
import com.xyauto.mapper.UserDetailInfoMapper;
import com.xyauto.mapper.UserExtMapper;
import com.xyauto.mapper.UserInfoMapper;
import com.xyauto.mapper.UserRoleMapper;
import com.xyauto.util.CacheUtil;
import com.xyauto.util.LogInfoUtil;
import com.xyauto.util.Md5Utils;
import com.xyauto.util.RestEntity;
import com.xyauto.util.StringUtil;
import com.xyauto.util.SysConstants;

@Service
public class UserService {

	@Value("${com.xyauto.controller.localPath}")
	private String localPath;
	@Value("${com.xyauto.controller.uploadImgPath}")
	private String uploadImgPath;
	@Value("${com.xyauto.controller.domainName}")
	private String domainName;
	@Value("${com.xyauto.service.defaultPassword}")
	private String defaultPassword;

	@Autowired
	private UserInfoMapper userInfoMapper;
	@Autowired
	private UserDetailInfoMapper userDetailInfoMapper;
	@Autowired
	private UserRoleMapper userRoleMapper;
	@Autowired
	private UserExtMapper userExtMapper;
	@Autowired
	private DictInfoMapper dictInfoMapper;
	@Autowired
	private LogInfoUtil logInfoUtil;
	@Autowired
	private UploadFileInfoMapper uploadFileInfoMapper;

	public Integer insertUserInfo(UserInfo userInfo) {
		return userInfoMapper.insert(userInfo);
	}

	public Integer deleteUserInfo(Integer id) {
		return userInfoMapper.deleteByPrimaryKey(id);
	}

	public Integer updateUserInfo(UserInfo userInfo, String ip) {

		return userInfoMapper.updateByPrimaryKey(userInfo);
	}

	public RestEntity<UserInfo> selectUserInfoOne(Integer id) {
		return RestEntity.data(userInfoMapper.selectByPrimaryKey(id));
	}

	public List<UserInfo> selectUserInfoAll() {
		return userInfoMapper.selectAll();
	}

	/*
	 * 获取用户信息
	 */
	public UserExt getInfoAndDetail(Integer id, UserInfo loginInfo, Object roleDetail, Boolean isLoginDetail) {
		// 获取权限
		String role = loginInfo.getRoleIDs();
		Boolean role_1_2_4_5 = SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_ADD.equals(role)
				|| SysConstants.ROLE_OPERATE.equals(role) || SysConstants.ROLE_AE.equals(role);
		Boolean role_1_4_5 = SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role)
				|| SysConstants.ROLE_AE.equals(role);
		UserExt userExt = new UserExt();
		// 新增
		userExt.setIsNew(true);
		// 编辑
		if (id != 0) {
			userExt = userExtMapper.infoAndDetail(id);
			// 日志
			logInfoUtil.insertUserLogInfo(loginInfo, "查询用户信息", SysConstants.SELECT, SysConstants.USER_ROLE_MANAGER);
			// 3-媒体主 权限 没有所在行业
			if (!role_1_2_4_5) {
				userExt.setBusinessID(null);
			}
			// 2-广告主、3-媒体主、4-运营 权限 没有允许广告主授权给我，不能看到联系电话 或者查看自己的信息时
			if (!(SysConstants.ROLE_AE.equals(role)) || isLoginDetail) {
				userExt.setIsAuthMTZ(null);
			}
			if (!role_1_4_5 && !isLoginDetail) {
				userExt.setMobile(null);
			}
			userExt.setAuthAETrueName(null);
			userExt.setAuthAEUserID(null);
			// 类别-个人 没有营业执照、没有组织机构代码证、没有身份证背面照片
			if (userExt.getType() == null || userExt.getType().equals(SysConstants.TYPE_2)) {
				userExt.setbLicenceURL(null);
				userExt.setOrganizationURL(null);
				userExt.setIdCardBackURL(null);
			}
			userExt.setEmployeeNumber(null);
			userExt.setIsAuthAE(null);
			userExt.setSource(null);
			userExt.setStatus(null);
			userExt.setSysUserID(null);
			userExt.setUserName(null);
			userExt.setIsNew(false);
		}
		// 放置权限
		userExt.setRole(role);
		userExt.setRoleDetail(roleDetail);
		// 是否是loginDetail
		userExt.setIsLoginDetail(isLoginDetail);
		// 图片服务器地址
		userExt.setDomainName(domainName);
		// 行业列表
		if (role_1_2_4_5) {
			userExt.setDictType_2(CacheUtil.getDictMapByType(2, dictInfoMapper));
		}
		return userExt;
	}

	/*
	 * 新增或编辑用户信息 2.0
	 */
	@Transactional
	public RestEntity<String> setInfoAndDetailSenior(UserExt userExt, UserInfo loginInfo, boolean isInsert) {
		// 获取权限
		String role = loginInfo.getRoleIDs();
		// 建立模型
		UserInfo userInfo = new UserInfo();
		UserDetailInfo userDetailInfo = new UserDetailInfo();
		UserRole userRole = new UserRole();

		// 判断类型
		if (userExt.getType() == null) {
			return RestEntity.error("请选择类型！");
		}
		if (userExt.getType().intValue() == 1001) {
			// 类型为企业
			if (StringUtil.isEmpty(userExt.getTrueName().trim())) {
				return RestEntity.error("请输入公司名称");
			}
			int countTrueName = userExtMapper.countTrueName(userExt.getTrueName().trim());
			if (isInsert) {
				if (countTrueName > 0) {
					return RestEntity.error("企业名称不能重复！");
				}
			} else {
				UserDetailInfo oldUserDetailInfo = userDetailInfoMapper.selectByPrimaryKey(userExt.getUserID());
				if (oldUserDetailInfo != null && !userExt.getTrueName().equals(oldUserDetailInfo.getTrueName())) {
					if (countTrueName > 0) {
						return RestEntity.error("企业名称不能重复！");
					}
				}
			}
			if (StringUtil.isEmpty(userExt.getContact().trim())) {
				return RestEntity.error("请输入联系人！");
			}
			// 非自己信息的时候
			if (!userExt.getIsLoginDetail()) {
				if (StringUtil.isEmpty(userExt.getMobile().trim())) {
					return RestEntity.error("请输入联系电话！");
				}
				UserInfo oldUserInfo = userInfoMapper.selectByPrimaryKey(userExt.getUserID());
				int mobileCount = userExtMapper.countMobileAndCategory(userExt.getMobile().trim(),
						userExt.getCategory());
				if (isInsert) {
					if (mobileCount > 0) {
						return RestEntity.error("联系电话不能重复！");
					}
				} else {
					if (!oldUserInfo.getMobile().equals(userExt.getMobile())) {
						if (mobileCount > 0) {
							return RestEntity.error("联系电话不能重复！");
						}
					}
				}
				userInfo.setMobile(userExt.getMobile().trim());
				userInfo.setUserName(userInfo.getMobile());
				if (StringUtil.isEmpty(userExt.getPwd().trim())) {
					return RestEntity.error("请输入密码！");
				}
				// 如果是修改并且原密码发生了改变或者新增
				if (isInsert || (!isInsert && !userExt.getPwd().trim().equals(oldUserInfo.getPwd()))) {
					// 加密
					String pwd = Md5Utils.getMD5(
							userExt.getPwd().trim() + userExt.getCategory() + SysConstants.LOGINPWDKEY,
							SysConstants.MD5CHARTSET);
					userInfo.setPwd(pwd);
				}
				if (SysConstants.ROLE_AE.equals(role) && userExt.getIsAuthMTZ() != null) {
					userInfo.setIsAuthMTZ(userExt.getIsAuthMTZ());
				}
			}
			if (StringUtil.isEmpty(userExt.getbLicenceURL())) {
				return RestEntity.error("请上传营业执照");
			}
			if (StringUtil.isEmpty(userExt.getIdCardFrontURL())) {
				return RestEntity.error("请上传法人身份证");
			}
			// 企业专属
			userDetailInfo.setBLicenceURL(userExt.getbLicenceURL());
			userDetailInfo.setOrganizationURL(userExt.getOrganizationURL());
			// userDetailInfo.setIdCardBackURL(userExt.getIdCardBackURL());
			userDetailInfo.setContact(userExt.getContact().trim());
		} else {
			// 类型为个人
			if (StringUtil.isEmpty(userExt.getTrueName().trim())) {
				return RestEntity.error("请输入真实姓名");
			}
			// 非编辑自己信息的时候
			if (!userExt.getIsLoginDetail()) {
				if (StringUtil.isEmpty(userExt.getMobile().trim())) {
					return RestEntity.error("请输入联系电话！");
				}
				UserInfo oldUserInfo = userInfoMapper.selectByPrimaryKey(userExt.getUserID());
				int mobileCount = userExtMapper.countMobileAndCategory(userExt.getMobile().trim(),
						userExt.getCategory());
				if (isInsert) {
					if (mobileCount > 0) {
						return RestEntity.error("联系电话不能重复！");
					}
				} else {
					if (!oldUserInfo.getMobile().equals(userExt.getMobile())) {
						if (mobileCount > 0) {
							return RestEntity.error("联系电话不能重复！");
						}
					}
				}
				userInfo.setMobile(userExt.getMobile().trim());
				userInfo.setUserName(userInfo.getMobile());
				if (StringUtil.isEmpty(userExt.getPwd().trim())) {
					return RestEntity.error("请输入密码！");
				}
				// 如果是修改并且原密码发生了改变或者新增
				if (isInsert || (!isInsert && !userExt.getPwd().trim().equals(oldUserInfo.getPwd()))) {
					// 加密
					String pwd = Md5Utils.getMD5(
							userExt.getPwd().trim() + userExt.getCategory() + SysConstants.LOGINPWDKEY,
							SysConstants.MD5CHARTSET);
					userInfo.setPwd(pwd);
				}
				if (SysConstants.ROLE_AE.equals(role) && userExt.getIsAuthMTZ() != null) {
					userInfo.setIsAuthMTZ(userExt.getIsAuthMTZ());
					if (!userExt.getIsAuthMTZ()) {
						userInfo.setIsAuthAE(false);
					}
				}
			}
			if (StringUtil.isEmpty(userExt.getIdCardFrontURL())) {
				return RestEntity.error("请上传手持身份证");
			}
		}
		// 是新增
		if (isInsert) {
			// 创建基本信息
			userInfo.setUserName(userExt.getMobile().trim());
			userInfo.setSource(SysConstants.SOURCE_1);
			userInfo.setIsAuthAE(false);
			userInfo.setStatus(SysConstants.STATUS_0);
			userInfo.setCategory(userExt.getCategory());
			if (SysConstants.ROLE_AE.equals(role)) {
				userInfo.setAuthAEUserID(loginInfo.getUserID());
			}
			userInfo.setCreateUserID(loginInfo.getUserID());
		} else {
			userInfo.setUserID(userExt.getUserID());
		}
		userInfo.setType(userExt.getType());
		userInfo.setLastUpdateUserID(loginInfo.getUserID());
		userInfo.setLastUpdateTime(new Date());
		// 创建详细信息
		userDetailInfo.setTrueName(userExt.getTrueName().trim());
		// 如果添加的是广告主就有行业
		if (userExt.getCategory().equals(SysConstants.CATEGORY_1)) {
			userDetailInfo.setBusinessID(userExt.getBusinessID());
		}
		userDetailInfo.setProvinceID(userExt.getProvinceID());
		userDetailInfo.setCityID(userExt.getCityID());
		userDetailInfo.setCounntyID(userExt.getCounntyID());
		userDetailInfo.setAddress(userExt.getAddress());
		userDetailInfo.setIdCardFrontURL(userExt.getIdCardFrontURL());
		// 是新增
		if (isInsert) {
			userDetailInfo.setCreateUserID(loginInfo.getUserID());
		}
		userDetailInfo.setLastUpdateTime(new Date());
		userDetailInfo.setLastUpdateUserID(loginInfo.getUserID());
		int userInfoResult = 0;
		// 如果新增就插入，否则修改
		if (isInsert) {
			userInfoResult = userInfoMapper.insert(userInfo);
			userDetailInfo.setUserID(userInfo.getUserID());
		} else {
			userDetailInfo.setUserID(userExt.getUserID());
			userInfoResult = userInfoMapper.updateByPrimaryKey(userInfo);
		}

		int userDetailInfoResult = userExtMapper.userDetailInfoUpdateOrInsert(userDetailInfo);
		if (userInfoResult + userDetailInfoResult != 2) {
			return RestEntity.error("保存失败");
		}
		// 是新增创建角色信息和上传信息
		if (isInsert) {
			userRole.setUserID(userInfo.getUserID());
			if (userExt.getCategory().equals(SysConstants.CATEGORY_1)) {
				userRole.setRoleID(SysConstants.ROLE_ADD);
			} else {
				userRole.setRoleID(SysConstants.ROLE_MEDIA);
			}
			userRole.setStatus(SysConstants.STATUS_0);
			userRole.setSysID(SysConstants.SYS_ID);
			userRole.setCreateUserID(loginInfo.getUserID());
			int userRoleResult = userRoleMapper.insert(userRole);
			if (userRoleResult != 1) {
				return RestEntity.error("保存失败");
			}
			if (userExt.getType().intValue() == 1001) {
				// 插入上传信息
				int blResult = addUploadFileInfo(SysConstants.UPLOAD_USER_BL, userInfo.getUserID(),
						userDetailInfo.getBLicenceURL(), userExt.getbLicenceFileSize(), loginInfo.getUserID());
				int idcResult = addUploadFileInfo(SysConstants.UPLOAD_USER_IDC, userInfo.getUserID(),
						userDetailInfo.getIdCardFrontURL(), userExt.getIdCardFrontFileSize(), loginInfo.getUserID());
				if (blResult + idcResult != 2) {
					return RestEntity.error("保存失败");
				}
				if (StringUtil.isNotEmpty(userExt.getOrganizationURL()) && userExt.getOrganizationFileSize() != null) {
					int oResult = addUploadFileInfo(SysConstants.UPLOAD_USER_O, userInfo.getUserID(),
							userDetailInfo.getOrganizationURL(), userExt.getOrganizationFileSize(),
							loginInfo.getUserID());
					if (oResult != 1) {
						return RestEntity.error("保存失败");
					}
				}
			} else {
				int idcResult = addUploadFileInfo(SysConstants.UPLOAD_USER_IDC, userInfo.getUserID(),
						userDetailInfo.getIdCardFrontURL(), userExt.getIdCardFrontFileSize(), loginInfo.getUserID());
				if (idcResult != 1) {
					return RestEntity.error("保存失败");
				}
			}
		} else {
			File file = null;
			if (userExt.getType().intValue() == 1001) {
				UploadFileInfo blFileInfo = userExtMapper.selectUserUpload(SysConstants.UPLOAD_USER_BL,
						SysConstants.USER_DETAIL_INFO, userInfo.getUserID());
				UploadFileInfo oFileInfo = userExtMapper.selectUserUpload(SysConstants.UPLOAD_USER_O,
						SysConstants.USER_DETAIL_INFO, userInfo.getUserID());
				UploadFileInfo idcFileInfo = userExtMapper.selectUserUpload(SysConstants.UPLOAD_USER_IDC,
						SysConstants.USER_DETAIL_INFO, userInfo.getUserID());
				if (blFileInfo != null && !blFileInfo.getFilePah().equals(userDetailInfo.getBLicenceURL())) {
					int deleteResult = uploadFileInfoMapper.deleteByPrimaryKey(blFileInfo.getRecID());
					int inserResult = addUploadFileInfo(SysConstants.UPLOAD_USER_BL, userInfo.getUserID(),
							userDetailInfo.getBLicenceURL(), userExt.getbLicenceFileSize(), loginInfo.getUserID());
					if (deleteResult + inserResult != 2) {
						return RestEntity.error("保存失败");
					}
					String pathName = localPath + blFileInfo.getFilePah();
					file = new File(pathName);
					file.delete();
				}
				if (oFileInfo != null && !oFileInfo.getFilePah().equals(userDetailInfo.getOrganizationURL())) {
					int deleteResult = uploadFileInfoMapper.deleteByPrimaryKey(oFileInfo.getRecID());
					int inserResult = addUploadFileInfo(SysConstants.UPLOAD_USER_O, userInfo.getUserID(),
							userDetailInfo.getOrganizationURL(), userExt.getOrganizationFileSize(),
							loginInfo.getUserID());
					if (deleteResult + inserResult != 2) {
						return RestEntity.error("保存失败");
					}
					String pathName = localPath + oFileInfo.getFilePah();
					file = new File(pathName);
					file.delete();
				}
				if (idcFileInfo != null && !idcFileInfo.getFilePah().equals(userDetailInfo.getIdCardFrontURL())) {
					int deleteResult = uploadFileInfoMapper.deleteByPrimaryKey(idcFileInfo.getRecID());
					int inserResult = addUploadFileInfo(SysConstants.UPLOAD_USER_IDC, userInfo.getUserID(),
							userDetailInfo.getIdCardFrontURL(), userExt.getIdCardFrontFileSize(),
							loginInfo.getUserID());
					if (deleteResult + inserResult != 2) {
						return RestEntity.error("保存失败");
					}
					String pathName = localPath + idcFileInfo.getFilePah();
					file = new File(pathName);
					file.delete();
				}
			} else {
				UploadFileInfo idcFileInfo = userExtMapper.selectUserUpload(SysConstants.UPLOAD_USER_IDC,
						SysConstants.USER_DETAIL_INFO, userInfo.getUserID());
				if (idcFileInfo != null && !idcFileInfo.getFilePah().equals(userDetailInfo.getIdCardFrontURL())) {
					int deleteResult = uploadFileInfoMapper.deleteByPrimaryKey(idcFileInfo.getRecID());
					int inserResult = addUploadFileInfo(SysConstants.UPLOAD_USER_IDC, userInfo.getUserID(),
							userDetailInfo.getIdCardFrontURL(), userExt.getIdCardFrontFileSize(),
							loginInfo.getUserID());
					if (deleteResult + inserResult != 2) {
						return RestEntity.error("保存失败");
					}
					String pathName = localPath + idcFileInfo.getFilePah();
					file = new File(pathName);
					file.delete();
				}
			}
		}
		if (isInsert) {
			// 新增日志
			logInfoUtil.insertUserLogInfo(loginInfo, "新增用户信息", SysConstants.INSERT, SysConstants.USER_ROLE_MANAGER);
		} else {
			// 修改日志
			logInfoUtil.insertUserLogInfo(loginInfo, "修改用户信息", SysConstants.UPDATE, SysConstants.USER_ROLE_MANAGER);
		}
		return RestEntity.data(userInfo.getUserID().toString());
	}

	/*
	 * 获取密码信息和AE授权情况
	 */
	public UserExt getInfoPassAndAEName(UserInfo loginInfo, Object roleDetail) {
		// 获取权限
		String role = loginInfo.getRoleIDs();
		UserExt userExt = userExtMapper.infoPassAndAEName(loginInfo.getUserID());
		// 查询日志
		logInfoUtil.insertUserLogInfo(loginInfo, "查询用户安全信息", SysConstants.SELECT, SysConstants.USER_ROLE_MANAGER);
		userExt.setMobile(userExt.getMobile().replaceAll("(\\d{3})\\d{4}(\\d{4})", "$1****$2"));
		// 放置权限
		userExt.setRole(role);
		userExt.setRoleDetail(roleDetail);
		return userExt;
	}

	/*
	 * 修改密码
	 */
	@Transactional
	public RestEntity<String> setInfoPassAndAEName(UserExt userExt, UserInfo loginInfo) {
		// 取权限
		String role = loginInfo.getRoleIDs();
		// 建立模型
		UserInfo userInfo = new UserInfo();
		UserInfo oldUserInfo = userInfoMapper.selectByPrimaryKey(loginInfo.getUserID());
		if (StringUtil.isNotEmpty(userExt.getPwd()) && StringUtil.isNotEmpty(userExt.getNewPwd())) {
			// 加密后比较旧密码
			String pwd = Md5Utils.getMD5(userExt.getPwd() + loginInfo.getCategory() + SysConstants.LOGINPWDKEY,
					SysConstants.MD5CHARTSET);
			if (!oldUserInfo.getPwd().equals(pwd)) {
				return RestEntity.error("旧密码错误！");
			}
			String newPwd = Md5Utils.getMD5(userExt.getNewPwd() + loginInfo.getCategory() + SysConstants.LOGINPWDKEY,
					SysConstants.MD5CHARTSET);
			userInfo.setPwd(newPwd);
		}

		userInfo.setUserID(loginInfo.getUserID());
		userInfo.setLastUpdateTime(new Date());
		userInfo.setLastUpdateUserID(loginInfo.getUserID());
		// 权限AE并且获得用户授权
		if (userExt.getIsAuthMTZ()) {
			userInfo.setIsAuthAE(userExt.getIsAuthAE());
		}
		int userInfoResult = userInfoMapper.updateByPrimaryKey(userInfo);
		// 日志
		logInfoUtil.insertUserLogInfo(loginInfo, "修改用户安全信息", SysConstants.UPDATE, SysConstants.USER_ROLE_MANAGER);
		if (userInfoResult != 1) {
			return RestEntity.error("修改信息失败");
		}
		return RestEntity.success("保存成功");
	}

	/*
	 * 查询手机号是否重复
	 */
	public boolean getCountMobileCategory(String mobile, String category) {
		int count = userExtMapper.countMobileAndCategory(mobile, category);
		if (count > 0) {
			return true;
		}
		return false;
	}

	/*
	 * 修改手机
	 */
	@Transactional
	public RestEntity<String> setMobile(String mobile, Integer userID, UserInfo loginInfo) {
		UserInfo userInfo = new UserInfo();
		userInfo.setUserID(userID);
		userInfo.setUserName(mobile);
		userInfo.setMobile(mobile);
		int result = userInfoMapper.updateByPrimaryKey(userInfo);
		// 修改日志
		logInfoUtil.insertUserLogInfo(loginInfo, "修改用户安全信息", SysConstants.UPDATE, SysConstants.USER_ROLE_MANAGER);
		if (result == 1) {
			return RestEntity.success("修改密码成功");
		}
		return RestEntity.error("修改密码失败");
	}

	// 查询一个内部用户信息
	public UserExt getEmployeeInfo(Integer userID, UserInfo loginInfo, Object roleDetail) {
		String role = loginInfo.getRoleIDs();
		UserExt userExt = new UserExt();
		if (userID != null && userID.intValue() != 0
				&& (SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role))) {
			userExt = userExtMapper.infoAndDetail(userID);
			// 查询日志
			logInfoUtil.insertUserLogInfo(loginInfo, "查询内部员工信息", SysConstants.SELECT, SysConstants.USER_ROLE_MANAGER);
			if (userExt != null) {
				userExt.setRoleDetail(roleDetail);
				userExt.setIsNew(false);
			}
			return userExt;
		} else {
			userExt.setRoleDetail(roleDetail);
			userExt.setIsNew(true);
			return userExt;
		}
	}

	@Transactional
	public RestEntity<String> setEmployeeInfo(UserExt userExt, UserInfo loginInfo) {
		String role = loginInfo.getRoleIDs();
		if (SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role)) {
			if (userExt.getIsNew()) {
				Integer employeeNumberCount = userExtMapper.countEmployeeNumber(userExt.getEmployeeNumber());
				if (employeeNumberCount > 0) {
					return RestEntity.error("该用户已存在，请重新输入！");
				}
				Integer mobileCount = userExtMapper.countMobile(userExt.getMobile());
				if (mobileCount > 0) {
					return RestEntity.error("手机号已注册，请重新输入！");
				}
				// 建立模型
				UserInfo userInfo = new UserInfo();
				UserDetailInfo userDetailInfo = new UserDetailInfo();
				UserRole userRole = new UserRole();

				userInfo.setUserName(userExt.getMobile());
				userInfo.setMobile(userExt.getMobile());
				// MD5密码
				String pwd = Md5Utils.getMD5(defaultPassword + SysConstants.CATEGORY_1 + SysConstants.LOGINPWDKEY,
						SysConstants.MD5CHARTSET);
				userInfo.setPwd(pwd);
				userInfo.setEmployeeNumber(userExt.getEmployeeNumber());
				userInfo.setEmail(userExt.getEmail());
				userInfo.setSysUserID(userExt.getSysUserID());
				userInfo.setSource(SysConstants.SOURCE_1);
				userInfo.setStatus(SysConstants.STATUS_0);
				userInfo.setIsAuthAE(false);
				userInfo.setType(SysConstants.TYPE_2);
				userInfo.setCreateUserID(loginInfo.getUserID());
				userInfo.setLastUpdateUserID(loginInfo.getUserID());
				userInfo.setCategory(SysConstants.CATEGORY_1);
				int userInfoResult = userInfoMapper.insert(userInfo);
				// 检查userID
				if (userInfo.getUserID() != null && userInfo.getUserID() != 0) {
					userRole.setUserID(userInfo.getUserID());
					userRole.setRoleID(userExt.getRole());
					userRole.setStatus(SysConstants.STATUS_0);
					userRole.setSysID(SysConstants.SYS_ID);
					userRole.setCreateUserID(loginInfo.getUserID());
					int userRoleResult = userRoleMapper.insert(userRole);
					userDetailInfo.setUserID(userInfo.getUserID());
					userDetailInfo.setTrueName(userExt.getTrueName());
					userDetailInfo.setCreateUserID(loginInfo.getUserID());
					userDetailInfo.setLastUpdateTime(new Date());
					userDetailInfo.setLastUpdateUserID(loginInfo.getUserID());
					int userDetailInfoResult = userDetailInfoMapper.insert(userDetailInfo);
					userInfo.setLastUpdateTime(new Date());
					// 日志
					logInfoUtil.insertUserLogInfo(loginInfo, "保存内部员工信息", SysConstants.INSERT,
							SysConstants.USER_ROLE_MANAGER);
					if (userInfoResult + userDetailInfoResult + userInfoMapper.updateByPrimaryKey(userInfo)
							+ userRoleResult != 4) {
						return RestEntity.error("保存失败");
					}
				}
			} else {
				UserRole userRole = new UserRole();
				userRole.setUserID(userExt.getUserID());
				userRole.setRoleID(userExt.getOldRoleID());
				userRole.setNewRoleID(userExt.getRole());
				if (!userExt.getOldRoleID().equals(userExt.getRole())) {
					if (SysConstants.ROLE_AE.equals(userExt.getOldRoleID())) {
						int count = userExtMapper.countAEUserID(userExt.getUserID());
						if (count > 0) {
							return RestEntity.error("不能修改角色");
						}
					}
					int userRoleResult = userRoleMapper.updateByPrimaryKey(userRole);
					// 日志
					logInfoUtil.insertUserLogInfo(loginInfo, "修改用户权限", SysConstants.UPDATE,
							SysConstants.USER_ROLE_MANAGER);
					if (userRoleResult != 1) {
						return RestEntity.error("保存失败");
					}
				}
			}
		} else {
			return RestEntity.error("没有权限");
		}
		return RestEntity.success("保存成功");
	}

	private int addUploadFileInfo(int type, Integer userID, String filePath, Integer fileSize, int loginID) {
		UploadFileInfo uploadFileInfo = new UploadFileInfo();
		uploadFileInfo.setType(type);
		uploadFileInfo.setRelationTableName(SysConstants.USER_DETAIL_INFO);
		uploadFileInfo.setRelationID(userID);
		uploadFileInfo.setFilePah(filePath);
		int index = filePath.lastIndexOf("/");
		if (index == -1) {
			return 0;
		}
		String[] fileAllName = filePath.replace("/", "").substring(index).split("\\.");
		if (fileAllName.length != 2) {
			return 0;
		}
		uploadFileInfo.setFileName(fileAllName[0]);
		uploadFileInfo.setExtendName("." + fileAllName[1]);
		uploadFileInfo.setFileSize(fileSize / 1024);
		uploadFileInfo.setCreaetUserID(loginID);

		return uploadFileInfoMapper.insert(uploadFileInfo);
	}
}
