package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class LogInfo {
	private Integer module;
	private Integer actionType;
	private String content;
	private String IP;
	private Integer userID;
	private Date createTime;
}
