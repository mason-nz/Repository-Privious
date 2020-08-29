package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class CarTitleListQuery {
	private Integer brandID;
	private Integer serialID;
	private String BrandName;
	private String SerialName;
	private String titles;
	private String createUserName;
	private Date modifiedTime;
	private String showName;
	private String showTime;

	public String getShowName() {
		if (null != SerialName && !"".equals(SerialName)) {
			this.showName = BrandName + "-" + SerialName;
			return this.showName;
		} else {
			return BrandName;
		}
	}
}
