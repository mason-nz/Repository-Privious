package com.xyauto.config;

import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.ApplicationEvent;
import org.springframework.context.ApplicationListener;
import org.springframework.context.event.ContextClosedEvent;

import com.xyauto.util.HttpUtil;

public class AppListener implements ApplicationListener<ApplicationEvent> {

	@Override
	public void onApplicationEvent(ApplicationEvent event) {
		if (event instanceof ApplicationReadyEvent) {
			// 启动需要初始化的内容放这里
			HttpUtil.getClient();
		} else if (event instanceof ContextClosedEvent) {
			// 退出需要关闭的内容放这里
			HttpUtil.closedHttp();
		}

	}

}
