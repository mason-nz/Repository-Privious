package com.xyauto.config;

import java.util.concurrent.Future;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.util.EntityUtils;
import org.springframework.util.StringUtils;
import org.springframework.web.servlet.handler.HandlerInterceptorAdapter;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.xyauto.domain.UserInfo;
import com.xyauto.util.CookiesUtil;
import com.xyauto.util.HttpUtil;
import com.xyauto.util.SysConstants;

import lombok.extern.slf4j.Slf4j;

@Slf4j
public class LoginInterceptor extends HandlerInterceptorAdapter {

	private static Object lock = new Object();
	private static ObjectMapper objectMapper;

	@Override
	public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler)
			throws Exception {
		long nowTime = System.currentTimeMillis();
		String url = request.getRequestURL().toString();
		log.debug(url);
		if (url.endsWith("/error") || url.endsWith("/userInfo/toRegister")
				|| url.endsWith("/userInfo/checkPhoneRegister") || url.endsWith("/userInfo/getMobileCode")
				|| url.indexOf("/userInfo/checkimagecode") != -1 || url.endsWith("/userInfo/registerForm")
				|| url.indexOf("/userInfo/getSysManageLoginCode") != -1
				|| url.endsWith("/userInfo/findMobileCode")
				|| url.endsWith("/userInfo/toRecoverPWD")
				|| url.endsWith("/userInfo/recoverPWDForm")
				|| url.endsWith("/userInfo/checkMobileCode")) {
			return true;
		}
		boolean res = true;
		HttpSession session = request.getSession();
		UserInfo userObject = null;
		String cookieValue = CookiesUtil.getCookieValue(request, SysConstants.COOKIE_NAME);
		if (!StringUtils.isEmpty(cookieValue)) {
			log.debug(cookieValue);
			userObject = getUserFromCookies(cookieValue);
			if (userObject == null) {
				session.invalidate();
			} else {
				session.setAttribute(SysConstants.SEESION_USER, userObject);
				log.debug("login " + userObject);
			}
		}
		if (userObject == null) {
			response.sendRedirect(SysConstants.LOGIN_URL);
			res = false;
		}
		log.info("LoginInterceptor time:" + (System.currentTimeMillis() - nowTime));
		return res;
	}

	private UserInfo getUserFromCookies(String cookieValue) throws Exception {
		CloseableHttpAsyncClient client = HttpUtil.getClient();
		long time = System.currentTimeMillis();
		HttpGet mHttpGet = new HttpGet(SysConstants.GET_USER_INFO);
		mHttpGet.addHeader("Cookie", "ct-uinfo=" + cookieValue);
		log.debug(mHttpGet.getURI().toString());
		Future<HttpResponse> future = client.execute(mHttpGet, null);
		HttpResponse mHttpResponse = future.get();
		String res = EntityUtils.toString(mHttpResponse.getEntity());
		log.debug(res);
		if (res.indexOf("{\"Status\":0") == -1) {
			return null;
		}
		log.debug("getUserFromCookies time:" + (System.currentTimeMillis() - time));
		return getUser(res);
	}

	private static UserInfo getUser(String jsonStr) {
		if (objectMapper == null) {
			synchronized (lock) {
				if (objectMapper == null) {
					objectMapper = new ObjectMapper();
				}
			}
		}
		try {
			jsonStr = jsonStr.substring(jsonStr.indexOf("[") + 1, jsonStr.indexOf("]"));
			return objectMapper.readValue(jsonStr, UserInfo.class);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return null;
	}
}