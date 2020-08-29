<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeCategory.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.HomeCategory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery.1.11.3.min.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #u305_input {
            height: 156px;
        }
        #u306_input {
            height: 159px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <!-- pnl选择分类 (动态面板) -->
      <div id="u301" class="ax_default" data-label="pnl选择分类">
        <div id="u301_state0" class="panel_state" data-label="State1">
          <div id="u301_state0_content" class="panel_state_content">

            <!-- Unnamed (矩形) -->
            <div id="u302" class="ax_default box_1">
              <div id="u302_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u303" class="text" style="display:none; visibility: hidden">
                <p><span></span></p>
              </div>
            </div>

            <!-- Unnamed (组合) -->
            <div id="u304" class="ax_default" data-width="593" data-height="170">

              <!-- Unnamed (列表框) -->
              <div id="u305" class="ax_default list_box">
                <select id="u305_input" size="2">
                  <option value="互联网">互联网</option>
                  <option value="新闻时政">新闻时政</option>
                  <option value="金融理财">金融理财</option>
                  <option value="教育培训">教育培训</option>
                  <option value="科技IT">科技IT</option>
                  <option value="汽车">汽车</option>
                  <option value="职场/管理">职场/管理</option>
                  <option value="娱乐/影视">娱乐/影视</option>
                  <option value="文学/艺术">文学/艺术</option>
                  <option value="体育/健身">体育/健身</option>
                  <option value="时尚搭配">时尚搭配</option>
                  <option value="亲子/育儿">亲子/育儿</option>
                  <option value="美食">美食</option>
                  <option value="美妆美体">美妆美体</option>
                  <option value="家居房产">家居房产</option>
                  <option value="旅游摄影 ">旅游摄影 </option>
                  <option value="情感心理">情感心理</option>
                  <option value="广告/营销">广告/营销</option>
                  <option value="个人媒体">个人媒体</option>
                  <option value="搞笑幽默">搞笑幽默</option>
                  <option value="创意/生活">创意/生活</option>
                  <option value="健康养生">健康养生</option>
                  <option value="游戏动漫">游戏动漫</option>
                  <option value="两性">两性</option>
                  <option value="其他">其他</option>
                </select>
                <input id="u307_input" type="submit" value="&lt;"/>
                <input id="u310_input" type="submit" value="&gt;"/>&nbsp;
                <select id="u306_input" size="2" name="D1">
                  <option value="时尚搭配">时尚搭配</option>
                  <option value="旅游摄影 ">旅游摄影 </option>
                  <option value="文学/艺术">文学/艺术</option>
                  <option value="美妆美体">美妆美体</option>
                  <option value="美食">美食</option>
                  <option value="亲子/育儿">亲子/育儿</option>
                  <option value="科技IT">科技IT</option>
                  <option value="金融理财">金融理财</option>
                </select><input id="u308_input" type="submit" value="上移"/><input id="u309_input" type="submit" value="下移"/></div>

              <!-- Unnamed (列表框) -->
              <div id="u306" class="ax_default list_box">
                &nbsp;</div>

              <!-- Unnamed (提交按钮) -->
              <div id="u307" class="ax_default html_button">
                &nbsp;<input id="u313_input" type="submit" value="确定"/><input id="u314_input" type="submit" value="取消"/></div>

              <!-- Unnamed (提交按钮) -->
              <div id="u308" class="ax_default html_button">
                &nbsp;</div>

              <!-- Unnamed (提交按钮) -->
              <div id="u309" class="ax_default html_button">
                &nbsp;</div>

              <!-- Unnamed (提交按钮) -->
              <div id="u310" class="ax_default html_button">
                &nbsp;</div>
            </div>

            <!-- Unnamed (矩形) -->
            <div id="u311" class="ax_default box_1">
              <div id="u311_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u312" class="text" style="display:none; visibility: hidden">
                <p><span></span></p>
              </div>
            </div>

            <!-- Unnamed (提交按钮) -->
            <div id="u313" class="ax_default html_button">
              &nbsp;</div>

            <!-- Unnamed (提交按钮) -->
            <div id="u314" class="ax_default html_button">
              &nbsp;</div>

            <!-- Unnamed (矩形) -->
            <div id="u315" class="ax_default box_1">
              <div id="u315_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u316" class="text">
                <p><span>&nbsp;&nbsp; 选择分类</span></p>
              </div>
            </div>

            <!-- Unnamed (矩形) -->
            <div id="u317" class="ax_default label">
              <div id="u317_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u318" class="text">
                <p><span>关闭</span></p>
              </div>
            </div>

            <!-- Unnamed (矩形) -->
            <div id="u319" class="ax_default label">
              <div id="u319_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u320" class="text">
                <p><span>未选分类</span></p>
              </div>
            </div>

            <!-- Unnamed (矩形) -->
            <div id="u321" class="ax_default label">
              <div id="u321_div" class=""></div>
              <!-- Unnamed () -->
              <div id="u322" class="text">
                <p><span>已选分类</span></p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    </form>
</body>
</html>
