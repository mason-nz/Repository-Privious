package com.xyauto.controller;

import java.util.Date;

import javax.servlet.http.HttpServletRequest;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.SessionAttribute;
import org.springframework.web.servlet.ModelAndView;

import com.xyauto.domain.UserExt;
import com.xyauto.domain.UserInfo;
import com.xyauto.oa.Employee;
import com.xyauto.oa.EmployeeService;
import com.xyauto.oa.EmployeeServiceSoap;
import com.xyauto.service.UserService;
import com.xyauto.util.AuthoUtil;
import com.xyauto.util.IpUtil;
import com.xyauto.util.RestEntity;
import com.xyauto.util.SMSUtil;
import com.xyauto.util.StringUtil;
import com.xyauto.util.SysConstants;

@Controller
@RequestMapping("userInfo/user")
public class UserController {

	@Value("${com.xyauto.controller.codeSize}")
	private String codeSize;

	@Autowired
	private UserService userService;

	@GetMapping(value = "getLoginDetail")
	public ModelAndView getLoginDetail(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		Object roleDetail = null;
		try {
			roleDetail = AuthoUtil.getAuthoMap(request,
					"SYS001BUT300101,SYS001BUT300102,SYS001BUT300103,SYS001BUT300104");
		} catch (Exception e) {
			e.printStackTrace();
			return new ModelAndView("hello");
		}
		UserExt userExt = userService.getInfoAndDetail(loginInfo.getUserID(), loginInfo, roleDetail, true);
		return new ModelAndView("userByPersonal", "userExt", userExt);
	}

	@GetMapping(value = "getInfoAndDetail/{category}/{id}")
	public ModelAndView getInfoAndDetail(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			@PathVariable Integer id, @PathVariable String category, HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		Object roleDetail = null;
		try {
			roleDetail = AuthoUtil.getAuthoMap(request,
					"SYS001BUT300101,SYS001BUT300102,SYS001BUT300103,SYS001BUT300104");
		} catch (Exception e) {
			e.printStackTrace();
			return new ModelAndView("hello");
		}
		UserExt userExt = userService.getInfoAndDetail(id, loginInfo, roleDetail, false);
		userExt.setCategory(category);
		return new ModelAndView("userByPersonal", "userExt", userExt);
	}

	@PostMapping(value = "setInfoAndDetailSenior")
	public @ResponseBody RestEntity<String> setInfoAndDetailSenior(
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo, @RequestBody UserExt userExt,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		// 获取权限
		String role = loginInfo.getRoleIDs();
		Boolean role_1_4_5 = SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role)
				|| SysConstants.ROLE_AE.equals(role);
		Boolean role_ALL = SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_ADD.equals(role)
				|| SysConstants.ROLE_MEDIA.equals(role) || SysConstants.ROLE_OPERATE.equals(role)
				|| SysConstants.ROLE_AE.equals(role) || SysConstants.ROLE_PLAN.equals(role);
		Integer userID = userExt.getUserID();
		if (userID == null && role_1_4_5) {
			return userService.setInfoAndDetailSenior(userExt, loginInfo, true);
		} else if (role_ALL) {
			return userService.setInfoAndDetailSenior(userExt, loginInfo, false);
		} else {
			return RestEntity.error("没有权限");
		}
	}

	@GetMapping(value = "getInfoPassAndAEName")
	public ModelAndView getInfoPassAndAEName(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		Object roleDetail = null;
		try {
			roleDetail = AuthoUtil.getAuthoMap(request, "SYS001BUT300201,SYS001BUT300202,SYS001BUT300203");
		} catch (Exception e) {
			e.printStackTrace();
			return new ModelAndView("hello");
		}
		UserExt userExt = userService.getInfoPassAndAEName(loginInfo, roleDetail);
		return new ModelAndView("userBySecurity", "userExt", userExt);
	}

	@PostMapping(value = "setInfoPassAndAEName")
	public @ResponseBody RestEntity<String> setInfoPassAndAEName(
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo, @RequestBody UserExt userExt,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		return userService.setInfoPassAndAEName(userExt, loginInfo);
	}

	@GetMapping(value = "getMessage/{mobile}")
	public @ResponseBody RestEntity<String> getMessage(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			@PathVariable String mobile, HttpServletRequest request) {
		if (userService.getCountMobileCategory(mobile, loginInfo.getCategory())) {
			return RestEntity.error("手机号已存在");
		}
		String code = StringUtil.getRandomString(Integer.parseInt(codeSize));
		// 发送手机验证码
		SMSUtil.sendSMS(mobile, SysConstants.PASS_MESSAGE.replace("X", code));
		request.getSession().setAttribute("mobile", mobile);
		request.getSession().setAttribute("code", code);
		request.getSession().setAttribute("time", new Date());
		return RestEntity.success("发送成功！");
	}

	@PostMapping(value = "setMobile")
	public @ResponseBody RestEntity<String> setMobile(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			@RequestBody String code, HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		String oldMobile = request.getSession().getAttribute("mobile").toString();
		String oldCode = request.getSession().getAttribute("code").toString();
		// 计算验证码失效时间
		Date oldTime = (Date) request.getSession().getAttribute("time");
		Date newTime = new Date();
		double result = (newTime.getTime() - oldTime.getTime()) / (1000 * 60.0);
		if (result > 3) {
			return RestEntity.error("验证码过期");
		}
		if (oldCode.equals(code.toLowerCase())) {
			return userService.setMobile(oldMobile, loginInfo.getUserID(), loginInfo);
		}
		return RestEntity.error("验证码错误");
	}

	/**
	 * in运营,out运营、AE、策划，快速注册内部员工和维护内部员工权限
	 */

	// 从OA系统获取内部员工信息
	@GetMapping(value = "getEmployee/{employeeNum}")
	public @ResponseBody RestEntity<Employee> getEmployee(
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo, @PathVariable String employeeNum,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		String role = loginInfo.getRoleIDs();
		if (SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role)) {
			EmployeeService mWmgw = new EmployeeService();
			EmployeeServiceSoap mSoap = mWmgw.getEmployeeServiceSoap();
			Employee res = mSoap.getEmployeeByEmployeeNumber(employeeNum);
			if (res == null) {
				return new RestEntity<Employee>(99, "没有找到对应的员工信息，请重新输入");
			}
			return RestEntity.data(res);
		}
		return new RestEntity<Employee>(99, "没有权限");
	}

	// 查询一个内部用户信息
	@GetMapping(value = "getEmployeeInfo/{userID}")
	public ModelAndView getEmployeeInfo(@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo,
			@PathVariable Integer userID, HttpServletRequest request) {
		String role = loginInfo.getRoleIDs();
		Object roleDetail = null;
		try {
			roleDetail = AuthoUtil.getAuthoMap(request,
					"SYS001MOD120010301,SYS001MOD120010302,SYS001MOD120010601,SYS001MOD120010602");
		} catch (Exception e) {
			e.printStackTrace();
			return new ModelAndView("hello");
		}
		if (SysConstants.ROLE_MANAGER.equals(role) || SysConstants.ROLE_OPERATE.equals(role)) {
			loginInfo.setIP(IpUtil.getIpAddr(request));
			UserExt userExt = userService.getEmployeeInfo(userID, loginInfo, roleDetail);
			if (userExt == null) {
				return new ModelAndView("hello");
			}
			return new ModelAndView("userByEmployee", "userExt", userExt);
		} else {
			return new ModelAndView("hello");
		}
	}

	@PostMapping(value = "setEmployeeInfo")
	public @ResponseBody RestEntity<String> setEmployeeInfo(
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo loginInfo, @RequestBody UserExt userExt,
			HttpServletRequest request) {
		loginInfo.setIP(IpUtil.getIpAddr(request));
		if (StringUtil.isEmpty(userExt.getEmployeeNumber())) {
			return RestEntity.error("请输入员工编号");
		}
		if (StringUtil.isEmpty(userExt.getTrueName())) {
			return RestEntity.error("请输入真是姓名");
		}
		if (StringUtil.isEmpty(userExt.getMobile())) {
			return RestEntity.error("请输入手机号");
		}
		if (StringUtil.isEmpty(userExt.getEmail())) {
			return RestEntity.error("请输入邮箱");
		}
		if (StringUtil.isEmpty(userExt.getRole()) || userExt.getRole().equals("-1")) {
			return RestEntity.error("请选择角色");
		}
		if (!userExt.getIsNew()) {
			if (userExt.getUserID() == null || userExt.getUserID() == 0) {
				return RestEntity.error("页面错误");
			}
		} else {
			if (userExt.getSysUserID() == null || userExt.getSysUserID() == 0) {
				return RestEntity.error("页面错误");
			}
		}
		return userService.setEmployeeInfo(userExt, loginInfo);
	}
}
