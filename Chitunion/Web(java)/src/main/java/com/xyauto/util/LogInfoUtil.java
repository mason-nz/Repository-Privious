package com.xyauto.util;

import java.util.Date;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.xyauto.domain.LogInfo;
import com.xyauto.domain.UserInfo;
import com.xyauto.mapper.LogInfoMapper;

@Component
public class LogInfoUtil {
	@Autowired
	private LogInfoMapper logInfoMapper;

	/**
	 * 用户管理日志模块：更新 param:message 日志操作内容 activeType 日志类型
	 */
	public void insertUserLogInfo(UserInfo userInfo, String message, Integer activeType, Integer module) {
		if (userInfo != null) {
			LogInfo logInfo = new LogInfo();
			logInfo.setUserID(userInfo.getUserID());
			logInfo.setContent("用户" + userInfo.getUserName() + "(ID:" + userInfo.getUserID() + ")" + message);
			logInfo.setIP(userInfo.getIP());
			logInfo.setCreateTime(new Date());
			logInfo.setActionType(activeType);
			logInfo.setModule(module);
			logInfoMapper.insertUserLog(logInfo);
		}
	}
}
