package com.xyauto.domain;

import lombok.Data;

@Data
public class ResultMessage {

	private Integer code;// 0-正常 1-为空 2-error 3-注册账号已存在
	private String message;
	private Integer userID;
	private String mobile;
}
