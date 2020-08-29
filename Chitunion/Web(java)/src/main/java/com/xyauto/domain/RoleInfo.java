package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class RoleInfo {

	private String roleID;
	private Integer recID;
	private String sysID;
	private String roleName;
	private String intro;
	private Integer status;
	private Date createTime;
	private Integer createUserID;
}