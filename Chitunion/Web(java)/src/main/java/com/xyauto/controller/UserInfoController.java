package com.xyauto.controller;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import lombok.extern.slf4j.Slf4j;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Controller;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.SessionAttribute;
import org.springframework.web.servlet.ModelAndView;

import com.xyauto.domain.RegisterValiMobile;
import com.xyauto.domain.ResultMessage;
import com.xyauto.domain.UserInfo;
import com.xyauto.domain.UserInfoOrder;
import com.xyauto.domain.UserInfoRequest;
import com.xyauto.service.UserInfoService;
import com.xyauto.util.AuthoUtil;
import com.xyauto.util.IpUtil;
import com.xyauto.util.MenuModile;
import com.xyauto.util.RandomValidateCode;
import com.xyauto.util.SMSUtil;
import com.xyauto.util.StringUtil;
import com.xyauto.util.SysConstants;

/**
 * @author xusy
 * @version 1.0
 */
@Controller
@RequestMapping("userInfo")
@Slf4j
public class UserInfoController {

	@Autowired
	private UserInfoService userInfoService;
	@Value("${com.xyauto.controller.codeSize}")
	private String codeSize;
	@Value("${integer.com.xyauto.controller.registerExpire}")
	private String registerExpire;
	@Value("${integer.com.xyauto.controller.recoverExpire}")
	private String recoverExpire;

	@RequestMapping(value = "toRegister")
	public String toRegister() {
		return "register";
	}

	@RequestMapping(value = "toRecoverPWD")
	public String toRecoverPWD() {
		return "recoverPWD";
	}

	@RequestMapping(value = "toUserOperateList")
	public ModelAndView toUserAdd(String userID, HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ModelAndView mv = new ModelAndView("hello");
		Map<String, Object> authoString = null;
		try {
			authoString = AuthoUtil.getAuthoMap(request,
					MenuModile.OPERATE_LIST_MENU);
		} catch (Exception e) {
			e.printStackTrace();
			log.error("Error Occured" + e);
			return mv;
		}
		mv.setViewName("userOperateList");
		mv.addObject("map", authoString);
		return mv;
	}

	@RequestMapping(value = "toUserMediaList")
	public ModelAndView toUserInfoMedia(HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ModelAndView mv = new ModelAndView("hello");
		Map<String, Object> authoString = null;
		try {
			authoString = AuthoUtil.getAuthoMap(request,
					MenuModile.MEDIA_LIST_MENU);
		} catch (Exception e) {
			e.printStackTrace();
			log.error("Error Occured" + e);
			return mv;
		}
		mv.setViewName("userMediaList");
		mv.addObject("map", authoString);
		return mv;
	}

	@RequestMapping(value = "toUserOperateAdd")
	public ModelAndView toUserOperateAdd(HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ModelAndView mv = new ModelAndView("hello");
		Map<String, Object> authoString = null;
		try {
			authoString = AuthoUtil.getAuthoMap(request,
					MenuModile.OPERATE_ADD_MENU);
		} catch (Exception e) {
			e.printStackTrace();
			log.error("Error Occured" + e);
			return mv;
		}
		mv.setViewName("userOperateAdd");
		mv.addObject("map", authoString);
		return mv;
	}

	@RequestMapping(value = "toUserOperateMedia")
	public ModelAndView toOperateMedia(String userID,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ModelAndView mv = new ModelAndView("hello");
		Map<String, Object> authoString = null;
		try {
			authoString = AuthoUtil.getAuthoMap(request,
					MenuModile.OPERATE_MEDIA_MENU);

		} catch (Exception e) {
			e.printStackTrace();
			log.error("Error Occured" + e);
			return mv;
		}
		mv.setViewName("userOperateMedia");
		mv.addObject("map", authoString);
		return mv;
	}

	/**
	 * 获取用户订单详情
	 * 
	 * @param userInfoRequest
	 * @param request
	 * @return
	 */
	@RequestMapping(value = "getUserInfoOrder", method = RequestMethod.POST)
	public @ResponseBody List<UserInfoOrder> getUserInfoOrder(
			@RequestBody UserInfoOrder userOrder, HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		if (loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)
				|| loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)) {
			userOrder.setCreateUserID(-1);
		} else if (loginUser.getRoleIDs().equals(SysConstants.ROLE_MEDIA)) {
			userOrder.setCreateUserID(loginUser.getUserID());
		} else {
			return null;
		}
		loginUser.setIP(IpUtil.getIpAddr(request));
		return userInfoService.getUserInfoOrder(userOrder, loginUser);
	}

	/**
	 * 指定AE，修改密码，修改用户状态
	 * 
	 * @param userInfoRequest
	 * @param request
	 * @return
	 */
	@RequestMapping(value = "upUserInfoByRquest", method = RequestMethod.POST)
	public @ResponseBody ResultMessage upUserInfoByRquest(
			@RequestBody UserInfoRequest userInfoRequest,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ResultMessage result = new ResultMessage();
		loginUser.setIP(IpUtil.getIpAddr(request));
		UserInfoRequest[] array = userInfoRequest.getArray();
		if (null != array && null != loginUser) {
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)) {
				userInfoService.upUserInfoByRquest(array, loginUser);
				result.setCode(0);
			}
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_AE)) {
				for (UserInfoRequest userInfoRequest2 : array) {
					userInfoRequest2.setAuthAEUserID(-1);
				}
				userInfoService.upUserInfoByRquest(array, loginUser);
				result.setCode(0);
			}
		} else {
			result.setCode(1);
			result.setMessage("空");
		}
		return result;
	}

	/**
	 * 获取全部AE角色
	 * 
	 * @return
	 */
	@RequestMapping(value = "/getAERole", method = RequestMethod.POST)
	public @ResponseBody List<UserInfoRequest> getAERole(
			@RequestBody UserInfoRequest userInfoRequest,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		List<UserInfoRequest> aeRole = null;
		loginUser.setIP(IpUtil.getIpAddr(request));
		if (null != loginUser) {
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)) {
				aeRole = userInfoService.getAERole(userInfoRequest, loginUser);
			}
		}
		return aeRole;
	}

	/**
	 * 修改用户authAE
	 * 
	 * @return
	 */
	@RequestMapping(value = "/updateUserAuthAE", method = RequestMethod.POST)
	public @ResponseBody ResultMessage updateUserAuthAE(
			@RequestBody UserInfoRequest userInfoRq,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		ResultMessage result = new ResultMessage();
		loginUser.setIP(IpUtil.getIpAddr(request));
		if (loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)
				|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)) {
			UserInfoRequest[] array = userInfoRq.getArray();
			if (null != array) {
				userInfoService.updateUserAuthAE(array, loginUser);
				result.setCode(0);
			} else {
				result.setCode(1);
				result.setMessage("AE不能为空");
			}
			return result;
		}
		return result;
	}

	/**
	 * 初始化用户列表数据
	 * 
	 * @return
	 */
	@RequestMapping(value = "/getUserInfo", method = RequestMethod.POST)
	public @ResponseBody Map<String, Object> getUserInfo(
			@RequestBody UserInfoRequest userInfoRequest,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		Map<String, Object> userInfoReq = null;
		if (loginUser != null) {
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_AE)) {
				if (userInfoRequest.getCategory() == 29001) {
					userInfoRequest.setRoleID(SysConstants.ROLE_ADD);
					userInfoRequest.setAuthAEUserID(loginUser.getUserID());
				}
				if (userInfoRequest.getCategory() == 29002) {
					userInfoRequest.setRoleID(SysConstants.ROLE_MEDIA);
					userInfoRequest.setAuthAEUserID(loginUser.getUserID());
				}
			}
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)) {
				if (userInfoRequest.getCategory() == 29001) {
					userInfoRequest.setRoleID(SysConstants.ROLE_ADD);
				}
				if (userInfoRequest.getCategory() == 29002) {
					userInfoRequest.setRoleID(SysConstants.ROLE_MEDIA);
				}
			}
			if (loginUser.getRoleIDs().equals(SysConstants.ROLE_ADD)
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MEDIA)
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_PLAN)
					|| loginUser.getRoleIDs() == null
					|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MEDIA)
					|| loginUser.getRoleIDs() == null
					|| loginUser.getRoleIDs().equals("")) {
				userInfoRequest.setRoleID("-1");
			}
			loginUser.setIP(IpUtil.getIpAddr(request));
			userInfoReq = userInfoService.getUserInfoReq(userInfoRequest,
					loginUser);
			return userInfoReq;
		}
		return userInfoReq;
	}

	/**
	 * 获取运营-用户管理-列表
	 * 
	 * @return
	 */
	@RequestMapping(value = "/getUserOperationList", method = RequestMethod.POST)
	public @ResponseBody Map<String, Object> getUserOperationList(
			@RequestBody UserInfoRequest userInfoRequest,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		loginUser.setIP(IpUtil.getIpAddr(request));
		Map<String, Object> userMap = null;
		if (loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)
				|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)) {
			if (null != userInfoRequest) {
				userMap = userInfoService.getUserOperationList(userInfoRequest,
						loginUser);
			}
		}
		return userMap;
	}

	/**
	 * 条件查询运营-用户管理-列表
	 * 
	 * @return
	 */
	@RequestMapping(value = "/selectAddUserList", method = RequestMethod.POST)
	public @ResponseBody Map<String, Object> selectAddUserList(
			@RequestBody UserInfoRequest userInfoRequest,
			HttpServletRequest request,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginUser) {
		loginUser.setIP(IpUtil.getIpAddr(request));
		Map<String, Object> userMap = null;
		if (loginUser.getRoleIDs().equals(SysConstants.ROLE_OPERATE)
				|| loginUser.getRoleIDs().equals(SysConstants.ROLE_MANAGER)) {
			if (null != userInfoRequest) {
				userMap = userInfoService.selectAddUserList(userInfoRequest,
						loginUser);
			}
		}
		return userMap;
	}

	/**
	 * 获取验证码
	 */
	@RequestMapping("/getSysManageLoginCode")
	@ResponseBody
	public String getSysManageLoginCode(HttpServletResponse response,
			HttpServletRequest request) {
		RandomValidateCode randomValidateCode = new RandomValidateCode();
		try {
			randomValidateCode.getRandcode(request, response, "imagecode");// 输出图片方法
		} catch (Exception e) {
			e.printStackTrace();
			log.error("Error Occured" + e);
		}
		return "";
	}

	// 验证码验证
	@RequestMapping(value = "/checkimagecode")
	@ResponseBody
	public ResultMessage checkTcode(HttpServletRequest request,
			HttpServletResponse response,
			@SessionAttribute("imagecode") String imagecode) {
		ResultMessage result = new ResultMessage();
		String validateCode = request.getParameter("validateCode");
		if (null != validateCode) {
			if (!StringUtils.isEmpty(validateCode)
					&& validateCode.equalsIgnoreCase(imagecode)) {
				result.setCode(0);
				return result;
			}
		}
		if (null != imagecode) {
			request.getSession().removeAttribute("imagecode");
		}
		result.setCode(1);
		result.setMessage("验证码错误，请输入新的验证码");
		return result;
	}

	// 手机号是否注册
	@RequestMapping(value = "/checkPhoneRegister", method = RequestMethod.POST)
	@ResponseBody
	public ResultMessage checkPhoneRegister(
			@RequestBody UserInfoRequest userInfo, HttpServletRequest request) {
		ResultMessage result = new ResultMessage();
		if (null != userInfo.getMobile()
				&& !userInfo.getMobile().trim().equals("")) {
			// Pattern p = Pattern
			// .compile("^((13[0-9])|(15[^4,\\D])|(18[0,5-9]))\\d{8}$");
			// Matcher m = p.matcher(userInfo.getMobile());
			// if (!m.find()) {
			// result.setCode(2);
			// result.setMessage("请输入正确的手机号");
			// return result;
			// } else {
			List<UserInfoRequest> checkMobileRegister = userInfoService
					.checkMobileRegister(userInfo);
			if (null != checkMobileRegister && checkMobileRegister.size() > 0) {
				result.setCode(3);
				result.setMessage("");
			} else {
				result.setCode(0);
				result.setMessage("");
			}
			// }
		} else {
			result.setCode(1);
			result.setMessage("请输入手机号");
		}

		return result;
	}

	/*
	 * 找回密码--提交
	 */
	@RequestMapping(value = "recoverPWDForm", method = RequestMethod.POST)
	public @ResponseBody ResultMessage recoverPWDForm(
			@RequestBody UserInfoRequest userInfo, HttpServletRequest request) {
		ResultMessage result = new ResultMessage();
		UserInfo loginUser = new UserInfo();
		loginUser.setIP(IpUtil.getIpAddr(request));
		if (userInfo == null || userInfo.getUserID() == null
				|| userInfo.getMobile() == null
				|| userInfo.getMobile().trim().equals("")
				|| userInfo.getCategory() == null)
			return result;
		if (checkPWD(userInfo.getPwd(), userInfo.getPwdTwo()).getCode()
				.intValue() != 0)
			return checkPWD(userInfo.getPwd(), userInfo.getPwdTwo());
		List<UserInfoRequest> userMobile = userInfoService
				.checkMobileRegister(userInfo);
		if (userMobile == null || userMobile.size() == 0) {
			result.setCode(6);
			return result;
		}
		userInfoService.updateUserPWD(userInfo, loginUser);
		result.setCode(0);
		return result;
	}

	/**
	 * 找回密码-获取手机验证码
	 * 
	 * @param userInfo
	 * @param request
	 * @param imagecode
	 * @return
	 */
	@RequestMapping(value = "/findMobileCode", method = RequestMethod.POST)
	@ResponseBody
	public ResultMessage findMobileCode(
			@RequestBody RegisterValiMobile userInfo,
			HttpServletRequest request,
			@SessionAttribute("imagecode") String imagecode) {
		ResultMessage result = new ResultMessage();
		if (null != userInfo) {
			if (imagecode.equalsIgnoreCase(userInfo.getValidateCode())) {
				// 生成随机验证码
				String mobileCode = "";
				mobileCode = StringUtil.getRandomString(Integer
						.parseInt(codeSize));
				String curDate = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss")
						.format(new Date());
				request.getSession().setAttribute(userInfo.getMobile(),
						mobileCode + "_" + curDate);
				// 发送手机验证码
				SMSUtil.sendSMS(userInfo.getMobile(),
						SysConstants.RECOVER_PWD_MESSAGE.replace("X",
								mobileCode));
				log.debug("--->sessionValue:"
						+ mobileCode
						+ "_"
						+ curDate
						+ "--sms_message:"
						+ SysConstants.RECOVER_PWD_MESSAGE.replace("X",
								mobileCode));
				result.setCode(0);
				return result;
			} else {
				result.setCode(1);
				result.setMessage("验证码错误，请输入新的验证码");
				if (null != imagecode) {
					request.getSession().removeAttribute("imagecode");
				}
				return result;
			}
		}
		result.setCode(1);
		result.setMessage("请输入验证码");
		return result;
	}

	// 获取手机验证码
	@RequestMapping(value = "/getMobileCode", method = RequestMethod.POST)
	@ResponseBody
	public ResultMessage getMobileCode(
			@RequestBody RegisterValiMobile userInfo,
			HttpServletRequest request,
			@SessionAttribute("imagecode") String imagecode) {
		ResultMessage result = new ResultMessage();
		if (null != userInfo) {
			if (imagecode.equalsIgnoreCase(userInfo.getValidateCode())) {
				// 生成随机验证码
				String mobileCode = "";
				mobileCode = StringUtil.getRandomString(Integer
						.parseInt(codeSize));
				String curDate = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss")
						.format(new Date());
				request.getSession().setAttribute(userInfo.getMobile(),
						mobileCode + "_" + curDate);
				// 发送手机验证码
				SMSUtil.sendSMS(userInfo.getMobile(),
						SysConstants.SMS_MESSAGE.replace("X", mobileCode));
				log.debug("--->sessionValue:" + mobileCode + "_" + curDate
						+ "--sms_message:"
						+ SysConstants.SMS_MESSAGE.replace("X", mobileCode));
				result.setCode(0);
				return result;
			} else {
				result.setCode(1);
				result.setMessage("验证码错误，请输入新的验证码");
				if (null != imagecode) {
					request.getSession().removeAttribute("imagecode");
				}
				return result;
			}
		}
		result.setCode(1);
		result.setMessage("请输入验证码");
		return result;
	}

	/**
	 * 正则验证密码
	 */
	private ResultMessage checkPWD(String newPWD, String confirmPWD) {
		ResultMessage result = new ResultMessage();
		if (null == newPWD) {
			result.setCode(1);// 请设置密码
			return result;
		} else {
			Matcher m = Pattern.compile("^[a-zA-Z\\d\\.@]{6,20}$").matcher(
					newPWD);
			if (false == m.find()) {
				result.setCode(2);// "密码长度应为6~20位字符"
				return result;
			}
		}
		if (null == confirmPWD) {
			result.setCode(4);// 请再次输入密码
			return result;
		}
		if (!newPWD.equals(confirmPWD)) {
			result.setCode(5);// "两次密码不一致"
			return result;
		}
		result.setCode(0);// 成功
		return result;
	}

	/**
	 * 验证手机号，手机验证码
	 * 
	 * @return
	 */
	private ResultMessage checkMobile(String mobile, String mobileCode) {
		ResultMessage result = new ResultMessage();
		if (null == mobile) {
			result.setCode(1);
			result.setMessage("请输入手机号");
			return result;
		}
		/*
		 * else { Pattern p = Pattern
		 * .compile("^((13[0-9])|(15[^4,\\D])|(18[0,5-9]))\\d{8}$"); Matcher m =
		 * p.matcher(userInfo.getMobile()); if (!m.find()) { result.setCode(1);
		 * result.setMessage("请输入正确的手机号"); return result; } }
		 */
		if (null == mobileCode) {
			result.setCode(1);
			result.setMessage("请输入手机验证码");
			return result;
		}
		result.setCode(0);
		return result;
	}

	/**
	 * 找回密码--验证手机验证码(第一步)
	 * 
	 * @param userInfo
	 * @param request
	 * @return
	 */
	@RequestMapping(value = "/checkMobileCode", method = RequestMethod.POST)
	@ResponseBody
	public ResultMessage checkMobileCode(@RequestBody UserInfoRequest userInfo,
			HttpServletRequest request) {
		ResultMessage result = new ResultMessage();
		if (checkMobile(userInfo.getMobile(), userInfo.getMobileCode())
				.getCode().intValue() != 0)
			return checkMobile(userInfo.getMobile(), userInfo.getMobileCode());
		List<UserInfoRequest> userMobile = userInfoService
				.checkMobileRegister(userInfo);
		if (userMobile == null || userMobile.size() == 0) {
			result.setCode(6);
			return result;
		}
		Object user = request.getSession().getAttribute(userInfo.getMobile());
		// 判断验证码有效性
		SimpleDateFormat sd = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		if (null != user) {
			String code = null;
			String codeTime = null;
			String format = null;
			Long min = null;
			try {
				code = user.toString().split("_")[0];
				codeTime = user.toString().split("_")[1];
				format = sd.format(new Date());
				long betweenTime = sd.parse(format).getTime()
						- sd.parse(codeTime).getTime();
				long nd = 1000 * 24 * 60 * 60;
				long nh = 1000 * 60 * 60;
				long nm = 1000 * 60;
				min = betweenTime % nd % nh / nm;
			} catch (ParseException e) {
				e.printStackTrace();
				log.error("Error Occured" + e);
			}
			log.debug("--->begin:" + codeTime + "---endTime:" + format
					+ "---betweenTime:" + min);
			if (min == null) {
				result.setCode(1);
				result.setMessage("请输入验证码");
				return result;
			}
			if (min >= Integer.parseInt(recoverExpire)) {// 验证码过期
				if (null != userInfo.getMobile())
					request.getSession().removeAttribute(userInfo.getMobile());
				result.setCode(1);
				result.setMessage("手机验证码已过期,请重新获取");
				return result;
			} else {
				// 验证码是否正确
				if (userInfo.getMobileCode().trim().equals(code)) {
					result.setCode(0);
					result.setUserID(userMobile.get(0).getUserID());
					result.setMobile(userMobile.get(0).getMobile());
					return result;
				} else {
					result.setCode(1);
					result.setMessage("手机验证码不正确，请重新输入");
					return result;
				}
			}
		} else {
			result.setCode(1);
			result.setMessage("请获取短信验证码");
			return result;
		}
	}

	// 注册提交
	@RequestMapping(value = "/registerForm", method = RequestMethod.POST)
	@ResponseBody
	public ResultMessage registerForm(@RequestBody UserInfoRequest userInfo,
			HttpServletRequest request) throws ParseException {
		Object user = request.getSession().getAttribute(userInfo.getMobile());
		ResultMessage result = new ResultMessage();
		userInfo.setIP(request.getRemoteAddr());
		if (null == userInfo.getMobile()) {
			result.setCode(1);
			result.setMessage("请输入手机号");
			return result;
		}
		/*
		 * else { Pattern p = Pattern
		 * .compile("^((13[0-9])|(15[^4,\\D])|(18[0,5-9]))\\d{8}$"); Matcher m =
		 * p.matcher(userInfo.getMobile()); if (!m.find()) { result.setCode(1);
		 * result.setMessage("请输入正确的手机号"); return result; } }
		 */
		if (null == userInfo.getPwd()) {
			result.setCode(1);
			result.setMessage("请设置密码");
			return result;
		} else {
			String regExp = "^[a-zA-Z\\d\\.@]{6,20}$";
			Pattern p = Pattern.compile(regExp);
			Matcher m = p.matcher(userInfo.getPwd());
			if (false == m.find()) {
				result.setCode(1);
				result.setMessage("密码长度应为6~20位字符");
				return result;
			}
		}
		if (null == userInfo.getPwdTwo()) {
			result.setCode(1);
			result.setMessage("请再次输入密码");
			return result;
		}
		if (!userInfo.getPwd().equals(userInfo.getPwdTwo())) {
			result.setCode(1);
			result.setMessage("两次密码不一致");
			return result;
		}
		if (null == userInfo.getMobileCode()) {
			result.setCode(1);
			result.setMessage("请输入手机验证码");
			return result;
		} else {
			List<UserInfoRequest> checkMobileRegister = userInfoService
					.checkMobileRegister(userInfo);
			if (null != checkMobileRegister && checkMobileRegister.size() > 0) {
				result.setCode(3);
				return result;
			} else {
				// 判断验证码有效性
				SimpleDateFormat sd = new SimpleDateFormat(
						"yyyy-MM-dd HH:mm:ss");
				if (null != user) {
					String code = null;
					String codeTime = null;
					String format = null;
					Long min = null;
					try {
						code = user.toString().split("_")[0];
						codeTime = user.toString().split("_")[1];
						format = sd.format(new Date());
						long betweenTime = sd.parse(format).getTime()
								- sd.parse(codeTime).getTime();
						long nd = 1000 * 24 * 60 * 60;
						long nh = 1000 * 60 * 60;
						long nm = 1000 * 60;
						min = betweenTime % nd % nh / nm;
					} catch (ParseException e) {
						e.printStackTrace();
						log.error("Error Occured" + e);
					}
					log.debug("--->begin:" + codeTime + "---endTime:" + format
							+ "---betweenTime:" + min);
					if (min == null) {
						result.setCode(1);
						result.setMessage("请输入验证码");
						return result;
					}
					if (min >= Integer.parseInt(registerExpire)) {// 验证码过期
						if (null != userInfo.getMobile())
							request.getSession().removeAttribute(
									userInfo.getMobile());
						result.setCode(1);
						result.setMessage("手机验证码已过期,请重新获取");
						return result;
					} else {
						// 验证码是否正确
						if (userInfo.getMobileCode().trim().equals(code)) {
							UserInfo loginUser = new UserInfo();
							loginUser.setIP(IpUtil.getIpAddr(request));
							userInfoService.insertUserInfo(userInfo, loginUser);
							result.setCode(0);
							result.setMessage("注册成功");
							return result;
						} else {
							result.setCode(1);
							result.setMessage("手机验证码不正确，请重新输入");
							return result;
						}
					}
				} else {
					result.setCode(1);
					result.setMessage("请获取短信验证码");
					return result;
				}
			}
		}
	}
}
