package com.xyauto.util;

import java.io.IOException;

import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.impl.nio.client.HttpAsyncClients;

import lombok.extern.slf4j.Slf4j;

@Slf4j
public class HttpUtil {

	private static Object lock = new Object();
	private static CloseableHttpAsyncClient client;

	public static CloseableHttpAsyncClient getClient() {
		if (client == null) {
			synchronized (lock) {
				if (client == null) {
					client = HttpAsyncClients.createDefault();
					log.debug("get new CloseableHttpAsyncClient");
				}
			}
		}
		if (!client.isRunning()) {
			client.start();
		}
		return client;
	}

	public static void closedHttp() {
		if (client != null) {
			try {
				client.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
			log.debug("CloseableHttpAsyncClient closed");
		}
	}

}
