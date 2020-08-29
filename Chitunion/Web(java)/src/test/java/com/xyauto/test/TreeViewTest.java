package com.xyauto.test;

import java.util.List;

import javax.annotation.Resource;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;

import com.xyauto.Application;
import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.mapper.TitleBasicInfoMapper;
import com.xyauto.mapper.TitleMediaMappingMapper;

@RunWith(SpringRunner.class)
@SpringBootTest(classes = Application.class)
public class TreeViewTest {
	@Resource
	private TitleBasicInfoMapper titlebasicinfoMapper;
	@Resource
	private TitleMediaMappingMapper titleMediaMappingMapper;

	@Before
	public void before() {
		System.out.println("测试开始");
	}

	@Test
	public void TestMapper() {
		List<TitleBasicInfo> list = titlebasicinfoMapper.selectAllTitle();
		org.junit.Assert.assertNotNull(list);
	}
	/*
	 * @Test public void TestgetWeiXinWithOutTitle(){ List<String> list =
	 * titleMediaMappingMapper.getWeiXinWithOutTitle();
	 * org.junit.Assert.assertNotNull(list); }
	 * 
	 * @Test public void TestgetOnlyMediaByWeiXinNumber(){
	 * List<MediaTitleListQuery> list =
	 * titleMediaMappingMapper.getOnlyMediaByWeiXinNumber("123");
	 * System.out.println(list); org.junit.Assert.assertNotNull(list); }
	 */

	@After
	public void after() {
		System.out.println("测试结束");
	}
}