package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class DictInfo {
	private Integer dictId;
	private Integer dictType;
	private String dictName;
	private Integer status;
	private Integer orderNum;
	private Date createTime;
}