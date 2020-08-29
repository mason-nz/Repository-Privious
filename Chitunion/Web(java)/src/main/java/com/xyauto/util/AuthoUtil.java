package com.xyauto.util;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Future;

import javax.servlet.http.HttpServletRequest;

import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.message.BasicHeader;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;

import com.fasterxml.jackson.databind.ObjectMapper;

/**
 * 
 * @param request
 * @param checkString
 * @return
 * @throws Exception
 * @author zhangmg
 */
public class AuthoUtil {

	@SuppressWarnings("unchecked")
	public static Map<String, Object> getAuthoMap(HttpServletRequest request, String checkString) throws Exception {
		ObjectMapper objectMapper = new ObjectMapper();
		HttpPost tHttpPost = new HttpPost(SysConstants.CHECK_RIGHT);
		tHttpPost.addHeader(HTTP.CONTENT_TYPE, "application/json");
		String cookieValue = CookiesUtil.getCookieValue(request, SysConstants.COOKIE_NAME);
		tHttpPost.addHeader("Cookie", "ct-uinfo=" + cookieValue);
		HashMap<String, String> authoMap = new HashMap<>();
		authoMap.put("ModuleIDs", checkString);
		String jsonStr = objectMapper.writeValueAsString(authoMap);
		StringEntity entity = new StringEntity(jsonStr);
		entity.setContentType("text/json");
		entity.setContentEncoding(new BasicHeader(HTTP.CONTENT_TYPE, "application/json"));
		tHttpPost.setEntity(entity);
		CloseableHttpAsyncClient client = HttpUtil.getClient();
		Future<HttpResponse> future = client.execute(tHttpPost, null);
		HttpResponse tHttpResponse = null;
		tHttpResponse = future.get();
		String res = EntityUtils.toString(tHttpResponse.getEntity());
		HashMap<String, Object> resultObject = objectMapper.readValue(res, HashMap.class);
		return (Map<String, Object>) resultObject.get("Result");
	}
}
