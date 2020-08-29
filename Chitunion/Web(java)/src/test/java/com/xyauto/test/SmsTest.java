package com.xyauto.test;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;

import com.xyauto.Application;
import com.xyauto.util.SMSUtil;

@RunWith(SpringRunner.class)
@SpringBootTest(classes = Application.class)
public class SmsTest {

	@Before
	public void before() {

	}

	@Test
	public void TestMapper() {
		// SMSUtil.sendSMS("1", "同事您好，感谢您对此次测试的配合。123456");
		System.out.println("game over");
	}

	@After
	public void after() {

	}
}