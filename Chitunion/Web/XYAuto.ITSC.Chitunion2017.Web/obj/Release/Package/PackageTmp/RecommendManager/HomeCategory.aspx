<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeCategory.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.RecommendManager.HomeCategory" %>

    <style type="text/css">
        .no_sort, .al_sort {
            width: 200px;
            height: 156px;
            float: left;
        }

        #u305_input {
            width: 200px;
            height: 156px;
            display: block;
        }

        #u306_input:hover #u305_input:hover {
            border: 1px solid #808080;
        }

        #u306_input {
            width: 200px;
            height: 156px;
            display: block;
        }

        .middle_btn, .right_btn {
            float: left;
            width: 50px;
            height: 156px;
            position: relative;
        }

        #u307_input {
            position: absolute;
            left: 8px;
            top: 45px;
            width: 35px;
        }

        #u308_input {
            position: absolute;
            left: 8px;
            top: 85px;
            width: 35px;
        }

        #u333_input {
            position: absolute;
            left: 8px;
            top: 50px;
            width: 45px;
            font-size:12px;
        }

        #u309_input {
            position: absolute;
            left: 8px;
            top: 85px;
            width: 45px;
            font-size:12px;
        }

       
    </style>
    <script>
        //3. 选中的到右边
        $("#u308_input").click(function () {
            $("#u306_input").append($("#u305_input>option:selected"));
        });
        //4. 选中的到左边
        $("#u307_input").click(function () {
            $("#u305_input").append($("#u306_input>option:selected"));
        });
        $("#u305_input").dblclick(function () {
            $("#u306_input").append($("#u305_input>option:selected"));
        });
        $("#u306_input").dblclick(function () {
            $("#u305_input").append($("#u306_input>option:selected"));
        });
        $("#u333_input").click(
            /** * 向上移动选中的option */
function () {
    if (null == $('#u306_input').val()) {
        alert('请选择一项');
        return false;
    }
    //选中的索引,从0开始 
    var optionIndex = $('#u306_input').get(0).selectedIndex;
    //如果选中的不在最上面,表示可以移动 
    if (optionIndex > 0) {
        $('#u306_input option:selected').insertBefore($('#u306_input option:selected').prev('option'));
    }
}
            );
        $("#u309_input").click(
            /** 
* 向下移动选中的option 
*/
function () {
    if (null == $('#u306_input').val()) {
        alert('请选择一项');
        return false;
    }
    //索引的长度,从1开始 
    var optionLength = $('#u306_input')[0].options.length;
    //选中的索引,从0开始 
    var optionIndex = $('#u306_input').get(0).selectedIndex;
    //如果选择的不在最下面,表示可以向下 
    if (optionIndex < (optionLength - 1)) {
        $('#u306_input option:selected').insertAfter($('#u306_input option:selected').next('option'));
    }
}
            )

    </script>

    <div class="layer" style="width:650px">
    <div class="title">
        <div class="fl">选择分类</div>
        <div class="fr"><a href="javascript:void(0)" id="closebt"><img src="../images/icon13.png" width="16" height="16" onmouseover="this.src='../images/icon14.png'" onmouseout="this.src='../images/icon13.png'"></a></div>
        <div class="clear"></div>
    </div>

    <div class="layer_con2" style="margin:0px;padding:20px;margin-top:-20px">
        <div class="layer_data">
        
        <div style="margin-top: 10px">
            <div id="u305" class="ax_default list_box">
                <div class="no_sort">

                    <span>未选分类：</span>
                    <select id="u305_input" size="2" multiple>
                        <%if (AllCategoryList != null && AllCategoryList.Rows.Count > 0)
                            {%>

                        <% foreach (System.Data.DataRow item in AllCategoryList.Rows)
                            { %>
                        <option style="padding-left:10px" value="<%=item["DictId"]%>"><%=item["DictName"] %></option>
                        <%}%>
                        <% }%>
                    </select>
                </div>
                <span class="middle_btn">
                    <button id="u307_input"><</button>
                    <button id="u308_input">> </button>
                </span>

                <div class="al_sort">
                    <span>已选分类：</span>
                    <select id="u306_input" size="2" multiple>
                        <%if (HomeCategoryList != null && HomeCategoryList.Rows.Count > 0)
                            {%>

                        <% foreach (System.Data.DataRow item in HomeCategoryList.Rows)
                            { %>
                        <option style="padding-left:10px" value="<%=item["CategoryID"] %>"><%=item["CategoryName"] %></option>
                        <%}%>

                        <% }%>
                    </select>
                </div>

                <span class="right_btn">
                    <button id="u333_input">上移</button>
                    <button id="u309_input">下移 </button>
                </span>
              

            </div>
        </div>
      

        </div>
    
        <div class="toast" style="text-align: left;padding-left: 30px;line-height: 16px;color: red;"></div>
        <div class="clear"></div>
        <div class="keep" style="margin-top:40px">
            <span><a href="#" class="button" style="width:150px">确定</a></span>
            <span><a href="#" class="but_keep" style="width:150px">取消</a></span>
        </div>

    </div>
</div>
