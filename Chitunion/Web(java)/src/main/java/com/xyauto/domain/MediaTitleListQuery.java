package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class MediaTitleListQuery {
	private Integer mediaId;
	private Integer mediaTypeId;
	private String mediaName;
	private String platNumber;
	private String mediaTypeName;
	private Date mediaCreateTime;
	private Date lastTitleCreateTime;
	private String createUserName;
	private String terminalId;
	private String platName;
	private String showTime;
	private String titles;
	private String headIconURL;

}
