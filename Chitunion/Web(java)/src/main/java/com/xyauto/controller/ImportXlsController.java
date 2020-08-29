package com.xyauto.controller;

import java.util.ArrayList;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.ss.usermodel.WorkbookFactory;
import org.apache.poi.ss.util.CellRangeAddress;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.domain.TitleImportPojo;
import com.xyauto.domain.UserInfo;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.service.ImportXlsService;
import com.xyauto.util.CacheUtil;
import com.xyauto.util.SysConstants;

import lombok.extern.slf4j.Slf4j;

@Controller
@RequestMapping("/uploadXls")
@Slf4j
public class ImportXlsController {
	@Resource
	private ImportXlsService importXlsService;
	@Resource
	private DictInfoMapper dictInfoMapper;

	@RequestMapping("/")
	public String toUpload() {
		return "upload";
	}

	@RequestMapping("/importToDatabase")
	@ResponseBody
	public String importXls(HttpServletRequest request, @RequestParam("titleFile") MultipartFile file,
			String importTitleToken, HttpServletResponse response) {
		String pTitleCate = CacheUtil.getDictMap(dictInfoMapper).get(31001).getDictName();

		String flag = "导入成功！！！";
		try {
			log.info("import title from xls to database");
			HttpSession session = request.getSession();
			String saveToken = (String) session.getAttribute("importTitleToken");
			if (null == saveToken || !importTitleToken.equals(saveToken)) {
				session.setAttribute("importTitleToken", importTitleToken);
			} else {
				flag = "请不要重复提交！！";
				return flag;
			}
			UserInfo user = (UserInfo) session.getAttribute(SysConstants.SEESION_USER);

			Workbook wb = null;
			int sheetIndex = 0;
			int startReadLine = 0;
			int tailLine = 0;

			wb = WorkbookFactory.create(file.getInputStream());

			Sheet sheet = wb.getSheetAt(sheetIndex);
			Row row = null;
			int lastRowNum = sheet.getLastRowNum();
			ArrayList<TitleImportPojo> rowList = new ArrayList<>();
			TitleImportPojo titleImportPojo = null;
			for (int i = startReadLine; i < lastRowNum - tailLine + 1; i++) {
				row = sheet.getRow(i);
				if (null == row) {
					continue;
				}
				Cell Mcell = row.getCell(0);
				Cell relationCell = row.getCell(1);
				Cell Zcell = row.getCell(2);
				String McellVal = getCellValue(Mcell);
				String ZcellVal = getCellValue(Zcell);
				// String relationCellVal = getCellValue(relationCell);
				boolean MisMerge = isMergedRegion(sheet, i, 0);
				boolean RisMerge = isMergedRegion(sheet, i, 1);
				if (MisMerge) {
					String rs = getMergedRegionValue(sheet, row.getRowNum(), Mcell.getColumnIndex());
					if ("".equals(rs) || null == rs) {
						continue;
					}
				}
				if (RisMerge) {
					String rs = getMergedRegionValue(sheet, row.getRowNum(), relationCell.getColumnIndex());
					if ("".equals(rs) || null == rs) {
						continue;
					}
				}
				if (pTitleCate.equals(McellVal) || null == Mcell) { // 判断第一列
					continue;
				}
				if (null == Zcell || "".equals(ZcellVal)) {
					continue;
				}

				titleImportPojo = new TitleImportPojo();
				for (Cell c : row) {
					int columnIndex = c.getColumnIndex();
					boolean isMerge = isMergedRegion(sheet, i, columnIndex);
					// 判断是否具有合并单元格
					if (isMerge) {
						String rs = getMergedRegionValue(sheet, row.getRowNum(), c.getColumnIndex());
						if ("".equals(rs) || null == rs) {
							continue;
						}
						if (0 == c.getColumnIndex()) { // 母ip
							titleImportPojo.setPTitleName(rs);
						}
						if (1 == c.getColumnIndex()) { // 权重
							titleImportPojo.setRelation(rs);
						}
					} else {
						String rs = getCellValue(c);
						if ("".equals(rs) || null == rs) {
							continue;
						} else if (0 == c.getColumnIndex()) { // 母ip
							titleImportPojo.setPTitleName(rs);
						} else if (1 == c.getColumnIndex()) { // 权重
							titleImportPojo.setRelation(rs);
						} else if (2 == c.getColumnIndex()) { // 子ip
							titleImportPojo.setKidTitleName(rs);
						} else { // 普通标签
							TitleBasicInfo titleBasicInfo = new TitleBasicInfo();
							titleBasicInfo.setName(rs);
							titleImportPojo.getBaseTitles().add(titleBasicInfo);
						}
						// System.out.print(c.getRichStringCellValue() + " ");
					}
				}
				rowList.add(titleImportPojo);
			}
			importXlsService.insertIntoDB(rowList, user);
			log.debug("import title successful !!!");
		} catch (Exception e) {
			flag = "读取失败，文件格式有问题！！";
			e.printStackTrace();
		}
		return flag;
	}

	/**
	 * CellRangeAddress ca = sheet.getMergedRegion(i); int firstRow =
	 * ca.getFirstRow(); int lastRow = ca.getLastRow(); int firstColumn =
	 * ca.getFirstColumn(); int lastColumn = ca.getLastColumn();
	 * log.debug(firstRow+":"+lastRow+":"+firstColumn+":"+lastColumn);
	 */

	/**
	 * 获取单元格的值
	 * 
	 * @param cell
	 * @return
	 */
	public String getCellValue(Cell cell) {

		if (cell == null)
			return "";

		if (cell.getCellType() == Cell.CELL_TYPE_STRING) {

			return cell.getStringCellValue();

		} else if (cell.getCellType() == Cell.CELL_TYPE_BOOLEAN) {

			return String.valueOf(cell.getBooleanCellValue());

		} else if (cell.getCellType() == Cell.CELL_TYPE_FORMULA) {

			return cell.getCellFormula();

		} else if (cell.getCellType() == Cell.CELL_TYPE_NUMERIC) {

			return String.valueOf(cell.getNumericCellValue());

		}
		return "";
	}

	/**
	 * 获取合并单元格的值
	 * 
	 * @param sheet
	 * @param row
	 * @param column
	 * @return
	 */
	public String getMergedRegionValue(Sheet sheet, int row, int column) {
		int sheetMergeCount = sheet.getNumMergedRegions();

		for (int i = 0; i < sheetMergeCount; i++) {
			CellRangeAddress ca = sheet.getMergedRegion(i);
			int firstColumn = ca.getFirstColumn();
			int lastColumn = ca.getLastColumn();
			int firstRow = ca.getFirstRow();
			int lastRow = ca.getLastRow();

			if (row >= firstRow && row <= lastRow) {

				if (column >= firstColumn && column <= lastColumn) {
					Row fRow = sheet.getRow(firstRow);
					Cell fCell = fRow.getCell(firstColumn);
					return getCellValue(fCell);
				}
			}
		}

		return null;
	}

	/**
	 * 判断指定的单元格是否是合并单元格
	 * 
	 * @param sheet
	 * @param row
	 *            行下标
	 * @param column
	 *            列下标
	 * @return
	 */
	private boolean isMergedRegion(Sheet sheet, int row, int column) {
		int sheetMergeCount = sheet.getNumMergedRegions();
		for (int i = 0; i < sheetMergeCount; i++) {
			CellRangeAddress range = sheet.getMergedRegion(i);
			int firstColumn = range.getFirstColumn();
			int lastColumn = range.getLastColumn();
			int firstRow = range.getFirstRow();
			int lastRow = range.getLastRow();
			if (row >= firstRow && row <= lastRow) {
				if (column >= firstColumn && column <= lastColumn) {
					return true;
				}
			}
		}
		return false;
	}

}
