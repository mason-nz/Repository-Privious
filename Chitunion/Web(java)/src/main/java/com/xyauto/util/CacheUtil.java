package com.xyauto.util;

import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import com.xyauto.domain.AreaInfo;
import com.xyauto.domain.DictInfo;
import com.xyauto.mapper.AreaInfoMapper;
import com.xyauto.mapper.DictInfoMapper;

import lombok.extern.slf4j.Slf4j;

@Slf4j
public class CacheUtil {

	private static Object dicLock = new Object();
	private static Object areaLock = new Object();
	private static Map<Integer, DictInfo> dicMap;
	private static Map<Integer, AreaInfo> areaMap;

	public static Map<Integer, DictInfo> getDictMap(DictInfoMapper mapper) {
		if (dicMap == null) {
			synchronized (dicLock) {
				if (dicMap == null) {
					List<DictInfo> dictInfoList = mapper.getDictInfoList();
					dicMap = new HashMap<>();
					for (DictInfo dictInfo : dictInfoList) {
						dicMap.put(dictInfo.getDictId(), dictInfo);
						log.debug(dictInfo.getDictName());
					}
				}
			}
		}
		return dicMap;
	}

	public static Map<Integer, AreaInfo> getAreaMap(AreaInfoMapper mapper) {
		if (areaMap == null) {
			synchronized (areaLock) {
				if (areaMap == null) {
					List<AreaInfo> areaInfoList = mapper.getAreaInfoList();
					areaMap = new HashMap<>();
					for (AreaInfo areaInfo : areaInfoList) {
						areaMap.put(areaInfo.getAreaID(), areaInfo);
						log.debug(areaInfo.getAreaName());
					}
				}
			}
		}
		return areaMap;
	}

	public static Map<Integer, String> getDictMapByType(Integer type, DictInfoMapper mapper) {
		Map<Integer, DictInfo> map = getDictMap(mapper);
		Map<Integer, String> result = new LinkedHashMap<Integer, String>();
		for (Map.Entry<Integer, DictInfo> entry : map.entrySet()) {
			if (entry.getValue().getDictType().intValue() == type.intValue()) {
				result.put(entry.getKey(), entry.getValue().getDictName());
			}
		}
		return result;
	}
}
