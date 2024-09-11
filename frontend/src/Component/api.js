import axios from "axios";
import { useEffect, useState } from "react";

// 로그인
export function userLogin(data) {
  const url = `/api/authenticate`;

  return axios.post(url, data);
}

let chanBear = "";

let headers = {
  headers: {
    accept: "application/json",
    Authorization: `Bearer ${
      sessionStorage.length != 0 ? sessionStorage.getItem("JWT-token") : ""
    }`,
  },
};

// 현재 로그인한 유저정보
export function getNowUser() {
  const url = `/user/current`;

  if (sessionStorage.length != 0) {
    //console.log(sessionStorage.getItem("JWT-token"));
    chanBear = sessionStorage.getItem("JWT-token");
    headers = {
      headers: {
        accept: "application/json",
        Authorization: `Bearer ${chanBear}`,
      },
    };
  }

  return axios.get(url, headers);
}
// 전체 유저정보
export function showAllUser() {
  const url = `/user/show`;

  return axios.get(url, headers);
}
export function showAllLectures() {
  const url = "/lectures/getalllectures";

  return axios.get(url);

  // 헤더에 뭘 붙이는 작업이었는데 까먹음(필요는 없음)
  // headers: { "Access-Control-Allow-Origin": "*" },
}

export function getLectureDetailById(id) {
  // let resultLecture = "";
  const url = `/lectures/getalllectures/${id}`;
  // try {
  //   const response = await axios.get(url);
  //   console.log("데이터", response.data);
  //   resultLecture = response;
  // } catch (error) {
  //   console.log("아이디 찾는 중 오류발생", error);
  // }
  // return resultLecture;

  return axios.get(url, headers);
}

// 회원가입 관련 api
export function userRegister(data) {
  const url = `/user/signup`;
  return axios.post(url, data);
}

// 마이페이지 관련 api
export function getMyLecture(id) {
  const url = `/api/products/subscribe/${id}`;
  console.log(headers);
  return axios.get(url, headers);
}

// 인기강의
export function getLectureTop4() {
  const url = `/api/products/subscribe/top4`;
  return axios.get(url, headers);
}

// 회원정보 수정
export function modifyData(id, data) {
  const url = `/user/modify/${id}`;
  return axios.patch(url, data, headers);
}

// 주식정보
export function showStock() {
  const API_KEY =
    "Bbw%2BKP6%2Bcl7za25F0EmakNrwYPJ%2FectOS5l7qDNt6AFKqEs2peyJUombGy4yhUNZ3Fz3chTfRzkVPNeNgEcjVg%3D%3D";

  const url = `https://apis.data.go.kr/1160100/service/GetStockSecuritiesInfoService/getStockPriceInfo?serviceKey=${API_KEY}&resultType=json&numOfRows=5&likeItmsNm=신`;

  return axios.get(url);
}

// 강의 구독하기
export function saveLecture(data) {
  const url = `/api/products/subscribe`;

  return axios.post(url, data, headers);
}

// faq
export function showFAQ() {
  const url = `/api/faq`;

  return axios.get(url, headers);
}

// 로그아웃
export function userLogout() {
  // const url = `/user/logout`;
  // console.log(headers);
  sessionStorage.removeItem("JWT-token");
  window.location.reload();
  // return axios.post(url, {}, headers);
}

// 게임정보 저장
export function fetchGameData(id, data) {
  const url = `/game/update/${id}`;

  return axios.patch(url, data, headers);
}

// 게임정보 불러오기
export function getMyGameData(id) {
  const url = `/game/getmydata/${id}`;
  console.log(id);

  return axios.get(url, headers);
}
// 가구정보 불러오기
export function callFurniture(id) {
  const url = `/game/callfurniture/${id}`;

  return axios.get(url, headers);
}

// 가구 삭제(제대로 안됨)
export function deleteMyFuniture(dataObj, id) {
  const url = `/game/delfurniture/${id}`;
  console.log(dataObj);
  return axios.delete(url, {
    headers: { Authorization: headers.headers.Authorization },
    data: dataObj,
  });
}

// 좋아요 저장
export function saveLike(data) {
  const url = `/like/save`;

  return axios.post(url, data, headers);
}
// 좋아요 불러오기
export function showLike() {
  const url = `/like/all`;

  return axios.get(url);
}
// 특정인의 좋아요 불러오기
export function showSomeoneLike(id) {
  const url = `/like/getsomeonelike/${id}`;

  return axios.get(url, headers);
}

// 모든 게임 데이터 불러오기
export function allGameData() {
  const url = `/game/showall`;

  return axios.get(url, headers);
}

// 랜덤 게임 데이터 불러오기
export function randomGameData(id) {
  const url = `/game/randomvisit/${id}`;

  return axios.get(url, headers);
}

// 특정 게임 데이터 불러오기
export function someGameData(id) {
  const url = `/game/visit/${id}`;

  return axios.get(url, headers);
}

// 메뉴정보 불러오기
export function getMenuData() {
  const url = `/menu/getallmenu`;

  return axios.get(url, headers);
}

// 창업문의 저장
export function saveInquery(data) {
  const url = `/inquery/save`;
  return axios.post(url, data, headers);
}

// 최초 게임 실행 시 랜덤 레시피 부여
export function initialRecipe(id) {
  const url = `/recipe/initial/${id}`;
  return axios.get(url, headers);
}

// 타인 카페 방문하여 좋아요 누를 시 받게되는 랜덤 레시피
export function getRandomRecipe(id, target) {
  const url = `/recipe/addrandomrecipe/${id}/${target}`;

  return axios.get(url, headers);
}

// 게임에 보낼 내가 소유한 레시피
export function myRecipe(id) {
  const url = `/recipe/myrecipe/${id}`;

  return axios.get(url, headers);
}

// 게임 내 초대메세지 보내기
export function inviteMyCafe(data) {
  const url = `/message/send`;

  return axios.post(url, data, headers);
}

// 게임 내 메세지 확인
// 내가 보낸거
export function checkMySendMessage(id) {
  const url = `/message/getmysendmessage/${id}`;

  return axios.get(url, headers);
}

// 받은거
export function checkMyMessage(id) {
  const url = `/message/getmymessage/${id}`;

  return axios.get(url, headers);
}

// 유저검색
export function showSearchSomeone(id) {
  const url = `/user/showsomeone/${id}`;

  return axios.get(url, headers);
}
