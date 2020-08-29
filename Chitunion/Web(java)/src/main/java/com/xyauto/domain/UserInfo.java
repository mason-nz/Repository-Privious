package com.xyauto.domain;

import java.util.Date;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.Data;

@Data
public class UserInfo {

	@JsonProperty(value = "UserID")
	private Integer userID;
	@JsonProperty(value = "UserName")
	private String userName;
	@JsonProperty(value = "Mobile")
	private String mobile;
	private String pwd;
	private String email;
	@JsonProperty(value = "Type")
	private Integer type;
	@JsonProperty(value = "Source")
	private Integer source;
	private Boolean isAuthMTZ;
	private Boolean isAuthAE;
	private Integer authAEUserID;
	private Integer sysUserID;
	private String employeeNumber;
	private Integer status;
	private Date createTime;
	private Integer createUserID;
	private Date lastUpdateTime;
	private Integer lastUpdateUserID;
	private String mobileCode;
	private String IP;
	private String trueName;
	@JsonProperty(value = "Category")
	private String category;
	@JsonProperty(value = "RoleIDs")
	private String roleIDs;
	@JsonProperty(value = "BUTIDs")
	private String butIDs;

}