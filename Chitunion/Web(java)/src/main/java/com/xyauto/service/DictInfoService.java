package com.xyauto.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.xyauto.mapper.DictInfoMapper;

@Service
public class DictInfoService {

	@Autowired
	private DictInfoMapper dictInfoMapper;

}
