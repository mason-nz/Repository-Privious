package com.xyauto.util;

import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.concurrent.FutureCallback;
import org.apache.http.entity.ContentType;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.util.EntityUtils;

import lombok.extern.slf4j.Slf4j;

@Slf4j
public class SMSUtil {

	private static final String GET_PASS_URL = "http://api.sys.xingyuanauto.com/sms/GetPassSecret";
	private static final String SEND_MESSAGE_URL = "http://api.sys.xingyuanauto.com/sms/SendSMS";

	private static final String APPID = "3";

	/**
	 * 异步发短信功能
	 * 
	 * @param mobile
	 *            手机号,多个逗号隔开
	 * @param message
	 *            内容
	 */
	public static void sendSMS(String mobile, String message) {
		CloseableHttpAsyncClient client = HttpUtil.getClient();
		long time = System.currentTimeMillis();
		HttpGet mHttpGet = new HttpGet(GET_PASS_URL + "?appid=" + APPID + "&ticket=" + time);
		log.debug(mHttpGet.getURI().toString());
		client.execute(mHttpGet, new FutureCallback<HttpResponse>() {

			@Override
			public void completed(HttpResponse result) {
				try {
					String pass = EntityUtils.toString(result.getEntity());
					log.debug("GetPassSecret completed " + pass);
					if (pass.indexOf("{result:'False'") == -1) {
						sendsms(mobile, message, client, time, pass);
					} else {
						log.error("GetPassSecret completed " + pass);
					}

				} catch (Exception e) {
					e.printStackTrace();
				}
			}

			@Override
			public void failed(Exception ex) {
				log.error("GetPassSecret HttpGet is failed");
			}

			@Override
			public void cancelled() {
				log.warn("GetPassSecret HttpGet is cancelled");
			}

		});
	}

	private static void sendsms(String mobile, String message, CloseableHttpAsyncClient client, long time,
			String pass) {
		int count = mobile.split(",").length;
		HttpPost mPost = new HttpPost(SEND_MESSAGE_URL);
		mPost.setEntity(new StringEntity(
				"appid=" + APPID + "&passkey=" + pass.substring(1, pass.length() - 1) + "&notecount=" + count
						+ "&phonelist=" + mobile + "&" + "t=" + time + "&noteContent=" + message
						+ "&SendUserId=111&SendUserIp=127.0.0.1",
				ContentType.create("application/x-www-form-urlencoded", "UTF-8")));
		log.debug(mPost.getURI().toString());
		client.execute(mPost, new FutureCallback<HttpResponse>() {

			@Override
			public void completed(HttpResponse result) {
				try {
					String res = EntityUtils.toString(result.getEntity());
					if (res.indexOf("{result:'True',message:") > -1) {
						log.info("SendSMS completed");
					} else {
						log.error("SendSMS failed " + res);
					}
				} catch (Exception e) {
					e.printStackTrace();
				}
			}

			@Override
			public void failed(Exception ex) {
				log.error("SendSMS HttpGet is failed");
			}

			@Override
			public void cancelled() {
				log.warn("SendSMS HttpGet is cancelled");
			}
		});
	}

}
