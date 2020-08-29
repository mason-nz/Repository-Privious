package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class UserRole {

	private Integer userID;
	private String roleID;
	private Integer recID;
	private String sysID;
	private Integer status;
	private Date createTime;
	private Integer createUserID;
	private String newRoleID;
}