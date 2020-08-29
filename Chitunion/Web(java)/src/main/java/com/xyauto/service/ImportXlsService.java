package com.xyauto.service;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import javax.annotation.Resource;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.xyauto.domain.IpTitleInfo;
import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.domain.TitleImportPojo;
import com.xyauto.domain.UserInfo;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.mapper.ImportTitleMapper;
import com.xyauto.util.CacheUtil;

@Service
@Transactional
public class ImportXlsService {
	@Resource
	private ImportTitleMapper importTitleMapper;
	@Resource
	private DictInfoMapper dictInfoMapper;

	public void insertIntoDB(ArrayList<TitleImportPojo> rowList, UserInfo user) {
		String rWenHe = CacheUtil.getDictMap(dictInfoMapper).get(32001).getDictName();
		String rXiangGuan = CacheUtil.getDictMap(dictInfoMapper).get(32002).getDictName();
		String rFanYi = CacheUtil.getDictMap(dictInfoMapper).get(32003).getDictName();

		Date date = new Date();
		Integer userID = null;
		if (null != user) {
			userID = user.getUserID();
		}
		for (int i = 0; i < rowList.size(); i++) {
			TitleImportPojo titleImportPojo = rowList.get(i);

			// 一、存入TitleBasicInfo表
			// 1、处理一行母ip
			String ptitleName = titleImportPojo.getPTitleName();
			String kidTitleName = titleImportPojo.getKidTitleName();
			Integer kTitleId = importTitleMapper.selectTitleIdByName(kidTitleName);
			Integer pTitleId = importTitleMapper.selectTitleIdByName(ptitleName);
			TitleBasicInfo ptb = new TitleBasicInfo();
			if (null != pTitleId) {
				titleImportPojo.setPTitleId(pTitleId); // 一行的母ip的id
			} else {
				ptb.setName(ptitleName); // 母标签名字
				ptb.setStatus(0); // 母标签名字
				ptb.setType(31001); // 母标签的类型码
				ptb.setCreatetime(date);
				ptb.setCreateuserid(userID);
				importTitleMapper.insertIntoBasicTitleDB(ptb); // 将母ip入库
				titleImportPojo.setPTitleId(ptb.getTitleid()); // 将插入后返回的主键存入一行中
			}

			// 2、处理一行权重
			String relation = titleImportPojo.getRelation();
			if (null != relation && rWenHe.equals(relation)) {
				titleImportPojo.setRelationId(32001); // 一行的权重
			}
			if (null != relation && rXiangGuan.equals(relation)) {
				titleImportPojo.setRelationId(32002);
			}
			if (null != relation && rFanYi.equals(relation)) {
				titleImportPojo.setRelationId(32003);
			}

			// 3、处理一行子ip
			TitleBasicInfo ktb = new TitleBasicInfo();

			if (null != kTitleId) {
				titleImportPojo.setKidTitleId(kTitleId);
			} else {
				ktb.setType(31002);
				ktb.setStatus(0);
				ktb.setName(kidTitleName); // 子ip的名字
				ktb.setCreatetime(date);
				ktb.setCreateuserid(userID);
				importTitleMapper.insertIntoBasicTitleDB(ktb); // 子ip入库
				titleImportPojo.setKidTitleId(ktb.getTitleid());
			}

			// 4、处理普通标签
			List<TitleBasicInfo> baseTitles = titleImportPojo.getBaseTitles();
			for (int j = 0; j < baseTitles.size(); j++) {
				TitleBasicInfo batb = baseTitles.get(j);
				Integer batbId = importTitleMapper.selectTitleIdByName(batb.getName());
				if (null != batbId) {
					batb.setTitleid(batbId);
				} else {
					batb.setTitleid(batbId);
					batb.setType(31003);
					batb.setStatus(0);
					batb.setCreatetime(date);
					batb.setCreateuserid(userID);
					importTitleMapper.insertIntoBasicTitleDB(batb);
				}
			}

		}

		// 二、存入关系表IPTitleInfo

		for (TitleImportPojo titleImportPojo2 : rowList) {
			IpTitleInfo ipTitleInfo = new IpTitleInfo();
			Integer pTitleId = titleImportPojo2.getPTitleId();
			Integer kidTitleId = titleImportPojo2.getKidTitleId();
			Integer relationId = titleImportPojo2.getRelationId();
			List<TitleBasicInfo> baseTitles = titleImportPojo2.getBaseTitles();
			if (null == baseTitles || baseTitles.size() == 0) {
				ipTitleInfo.setPIp(pTitleId);
				ipTitleInfo.setSubIp(kidTitleId);
				ipTitleInfo.setCreateTime(date);
				ipTitleInfo.setCreateUserId(userID);
				ipTitleInfo.setStatus(0);
				ipTitleInfo.setRelation(relationId);
				importTitleMapper.insertIntoRelationDB(ipTitleInfo);
			}
			if (null != baseTitles && baseTitles.size() > 0) {
				for (TitleBasicInfo titleBasicInfo : baseTitles) {
					Integer titleid = titleBasicInfo.getTitleid();
					Integer num = importTitleMapper.selectRelationByPidAndKidAndbasicId(pTitleId, kidTitleId, titleid);
					// Integer num1 =
					// importTitleMapper.selectRelationByKidAndbasicId(
					// kidTitleId, titleid);
					if (num == 0) {
						ipTitleInfo.setPIp(pTitleId);
						ipTitleInfo.setSubIp(kidTitleId);
						ipTitleInfo.setTitleId(titleid);
						ipTitleInfo.setCreateTime(date);
						ipTitleInfo.setCreateUserId(userID);
						ipTitleInfo.setStatus(0);
						ipTitleInfo.setRelation(relationId);
						importTitleMapper.insertIntoRelationDB(ipTitleInfo);
					}
				}
			}

		}

	}

}
