package com.xyauto.util;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class CookiesUtil {

	private final static String DOMAIN = "chitunion.com";
	private final static String PATH = "/";

	public static boolean setCookieValue(HttpServletResponse response, String key, String value) {
		return setCookieValue(response, key, value, PATH, DOMAIN);
	}

	public static boolean setCookieValue(HttpServletResponse response, String key, String value, String path,
			String domain) {
		try {
			Cookie cookie = new Cookie(key, value);
			cookie.setPath(path);// very important
			cookie.setDomain(domain);
			cookie.setMaxAge(-1);
			response.addCookie(cookie);
			return true;
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
	}

	public static String getCookieValue(HttpServletRequest request, String key) {
		try {
			Cookie[] cookies = request.getCookies();
			if (cookies == null)
				return "";
			for (int i = 0; i < cookies.length; i++) {
				Cookie cookie = cookies[i];
				if (key.equals(cookie.getName()))
					return (cookie.getValue());
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return "";
	}

}
