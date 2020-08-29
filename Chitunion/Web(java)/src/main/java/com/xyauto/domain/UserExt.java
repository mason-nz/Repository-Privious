package com.xyauto.domain;

import java.util.Map;

import lombok.Data;

@Data
public class UserExt {

	private Integer userID;
	private String userName;
	private String mobile;
	private String pwd;
	private String email;
	private Integer type;
	private Integer source;
	private Boolean isAuthMTZ;
	private Boolean isAuthAE;
	private Integer authAEUserID;
	private Integer sysUserID;
	private String employeeNumber;
	private Integer status;
	private String trueName;
	private Integer businessID;
	private Integer provinceID;
	private Integer cityID;
	private Integer counntyID;
	private String contact;
	private String address;
	private String bLicenceURL;
	private String organizationURL;
	private String idCardFrontURL;
	private String idCardBackURL;
	private String category;
	private String authAETrueName;
	private String newPwd;
	private Boolean isNew;
	private String role;
	private Object roleDetail;
	private String oldRoleID;
	private Boolean isLoginDetail;
	private Map<Integer, String> dictType_2;
	private String domainName;
	private Integer bLicenceFileSize;
	private Integer organizationFileSize;
	private Integer idCardFrontFileSize;

	public String getbLicenceURL() {
		return bLicenceURL;
	}

	public void setbLicenceURL(String bLicenceURL) {
		this.bLicenceURL = bLicenceURL;
	}

	public Integer getbLicenceFileSize() {
		return bLicenceFileSize;
	}

	public void setbLicenceFileSize(Integer bLicenceFileSize) {
		this.bLicenceFileSize = bLicenceFileSize;
	}
}
