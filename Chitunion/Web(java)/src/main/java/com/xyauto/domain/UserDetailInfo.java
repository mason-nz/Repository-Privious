package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class UserDetailInfo {

	private Integer userID;
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
	private Date createTime;
	private Integer createUserID;
	private Date lastUpdateTime;
	private Integer lastUpdateUserID;
}