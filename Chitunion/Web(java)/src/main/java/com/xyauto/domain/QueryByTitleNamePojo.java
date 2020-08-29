package com.xyauto.domain;

import java.util.List;

import lombok.Data;

@Data
public class QueryByTitleNamePojo {
	private Integer queryTypeId;
	private String titleName;
	private Integer curPage;
	private Integer pageRows;
	private Integer count;
	private Integer hasAutho;
	private List<String> resultTitleName;
	private List<MediaTitleListQuery> mediaQueryResult;
}
