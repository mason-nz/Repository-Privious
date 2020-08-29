package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class TitleCarMapping {
	private Integer recId;
	private Integer titleId;
	private Integer carBrandId;
	private Integer CSID;
	private Integer status;
	private Date createTime;
	private Integer createUserId;
}
