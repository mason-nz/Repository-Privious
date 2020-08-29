package com.xyauto.domain;

import lombok.Data;

@Data
public class UserInfoOrder {

	private Integer createUserID;
	private String subOrderID;
	private Integer mediaType;
	private Integer status;
	private String mediaTypeName;
	private String orderName;
	private String beginEndTime;
	private String createTime;
	private Double totalAmount;
}