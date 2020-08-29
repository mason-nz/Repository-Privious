package com.xyauto.domain;

import java.util.ArrayList;
import java.util.List;

import lombok.Data;

@Data
public class TitleImportPojo {
	private Integer pTitleId;
	private String pTitleName;
	private String relation;
	private Integer relationId;
	private Integer kidTitleId;
	private String kidTitleName;
	private List<TitleBasicInfo> baseTitles = new ArrayList<TitleBasicInfo>();
}
