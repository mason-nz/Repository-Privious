package com.xyauto.domain;

import java.io.Serializable;
import java.util.Date;

import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
public class UserInfoRequest implements Serializable {
	private Integer userID;
	private String userIDPwd;
	private UserInfoRequest[] array;
	private Integer mark;
	private Integer category;
	private String IP;
	private String userIDs;
	private String userName;
	private String mobile;
	private String mobileCode;
	private String aeName;
	private String roleID;
	private String roleName;
	private String sysID;
	private String pwd;
	private String pwdTwo;
	private Boolean IsAuthMTZ;
	private String sysUserID;
	private String employeeNumber;
	private Integer createUserID;
	private Date lastUpdateTime;
	private Integer lastUpdateUserID;
	private String trueName;
	private boolean IsAuthAE;
	private Integer authAEUserID;
	private Integer provinceID;
	private String provinceName;
	private Integer counntyID;
	private String counntyName;
	private Integer cityID;
	private String cityName;
	private Integer businessID;
	private String businessName;
	private Integer source;
	private String sourceName;
	private Integer type;
	private String typeName;
	private String createStartTime;
	private String createEndTime;
	private Integer status;
	private String statusName;
	private Date createTime;
	private String address;
	private Integer curPage;
	private Integer pageRows;
	private Integer count;

	public UserInfoRequest(Integer userID, Integer authAEUserID) {
		this.userID = userID;
		this.authAEUserID = authAEUserID;
	}

}
