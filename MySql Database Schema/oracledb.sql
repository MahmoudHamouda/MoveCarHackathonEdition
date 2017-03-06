/*
Navicat MySQL Data Transfer

Source Server         : Local Mysql
Source Server Version : 100119
Source Host           : localhost:3306
Source Database       : oracledb

Target Server Type    : MYSQL
Target Server Version : 100119
File Encoding         : 65001

Date: 2017-03-06 12:06:32
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for car_log
-- ----------------------------
DROP TABLE IF EXISTS `car_log`;
CREATE TABLE `car_log` (
  `CAR_REQUEST_ID` int(11) NOT NULL,
  `CAR_NOTIFICATION_ID` varchar(255) DEFAULT NULL,
  `BLOCKED_USER` varchar(255) DEFAULT NULL,
  `BLOCKER_USER` varchar(255) DEFAULT NULL,
  `REQUEST_DATE` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of car_log
-- ----------------------------
INSERT INTO `car_log` VALUES ('1', '1', '1', '1', '2017-03-06 11:09:21');
INSERT INTO `car_log` VALUES ('123', '7357', 'amery87', 'amery87', '2017-03-06 11:11:36');
INSERT INTO `car_log` VALUES ('123', '7357', 'amery87', 'amery87', '2017-03-06 11:13:00');
INSERT INTO `car_log` VALUES ('123', '7357', 'amery87', 'amery87', '2017-03-06 11:13:59');
INSERT INTO `car_log` VALUES ('123', '7357', 'amery87', 'amery87', '2017-03-06 11:35:52');
INSERT INTO `car_log` VALUES ('123', '7357', 'amery87', 'amery87', '2017-03-06 11:41:30');

-- ----------------------------
-- Table structure for car_user_view
-- ----------------------------
DROP TABLE IF EXISTS `car_user_view`;
CREATE TABLE `car_user_view` (
  `USERNAME` varchar(255) NOT NULL,
  `CAR_USERNAME` varchar(255) NOT NULL,
  `EMPLOYEE_NAME` varchar(255) NOT NULL,
  `EMAIL` varchar(255) NOT NULL,
  `DEPARTMENT` varchar(255) NOT NULL,
  `MANAGER_NAME` varchar(255) DEFAULT NULL,
  `EXTENSION` varchar(255) NOT NULL,
  `WORK_MOBILE` varchar(255) NOT NULL,
  `PERSONAL_MOBILE` varchar(255) NOT NULL,
  `PLATE_NUMBER` varchar(255) NOT NULL,
  `Manager_EMAIL` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`USERNAME`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of car_user_view
-- ----------------------------
INSERT INTO `car_user_view` VALUES ('amery', 'amery87', 'yehia Amer', 'yehia.amer@emc.com', 'LET', 'Laurence Sexton', '5632', '+201006314438', '+201006314438', 'ببب111', 'yehia.amer@emc.com');
