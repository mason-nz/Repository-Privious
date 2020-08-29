package com.xyauto.controller;

import java.io.IOException;
import java.util.concurrent.Future;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.util.EntityUtils;
import org.springframework.stereotype.Controller;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.xyauto.util.CookiesUtil;
import com.xyauto.util.HttpUtil;
import com.xyauto.util.SysConstants;

import lombok.extern.slf4j.Slf4j;

@Controller
@Slf4j
public class IndexController {

	@RequestMapping("exit")
	public void exit(HttpServletRequest request, HttpServletResponse response) {
		HttpSession session = request.getSession(false);
		if (session != null) {
			session.invalidate();
		}
		try {
			response.sendRedirect(SysConstants.EXXT_URL);
		} catch (IOException e) {
			e.printStackTrace();
		}
		log.debug("session invalidate and user exit");
	}

	@RequestMapping("api/Authorize/GetMenuInfo")
	public @ResponseBody String api(HttpServletRequest request, HttpServletResponse response) {
		String cookieValue = CookiesUtil.getCookieValue(request, SysConstants.COOKIE_NAME);
		String res = "";
		if (!StringUtils.isEmpty(cookieValue)) {
			CloseableHttpAsyncClient client = HttpUtil.getClient();
			long time = System.currentTimeMillis();
			HttpGet mHttpGet = new HttpGet("http://www.chitunion.com/api/Authorize/GetMenuInfo");
			mHttpGet.addHeader("Cookie", "ct-uinfo=" + cookieValue);
			Future<HttpResponse> future = client.execute(mHttpGet, null);
			HttpResponse mHttpResponse;
			try {
				mHttpResponse = future.get();
				res = EntityUtils.toString(mHttpResponse.getEntity());
			} catch (Exception e) {
				e.printStackTrace();
			}
			log.debug("api/Authorize/GetMenuInfo time:" + (System.currentTimeMillis() - time));
		}
		return res;
	}

}
