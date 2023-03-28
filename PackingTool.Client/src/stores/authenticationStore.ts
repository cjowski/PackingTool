import { SessionStorage } from "quasar"
import { defineStore } from "pinia"
import { ref } from "vue"
import {
  OpenAPI,
  UserService,
  type AuthenticateResponse,
  type UserResponse,
} from "@/api"

export const useAuthenticationStore = defineStore("authentication", () => {
  let _userID = -1
  const _userAuthorized = ref(false)

  const userID = () => _userID
  const isAuthorized = () => _userAuthorized.value

  const tryAutoLogin = async () => {
    const userName = SessionStorage.getItem("userName") as string
    const password = SessionStorage.getItem("password") as string

    if (!userName || !password) {
      return false
    }

    return (await login(userName, password)).success
  }

  const login = async (userName: string, password: string) => {
    const response = (await UserService.postApiUserAuthenticate({
      requestBody: { userName: userName, password: password },
    })) as AuthenticateResponse

    if (response.success) {
      _userID = response.userID
      _userAuthorized.value = true
      OpenAPI.TOKEN = response.token!

      SessionStorage.set("userName", userName)
      SessionStorage.set("password", password)
    }

    return response
  }

  const register = async (
    userName: string,
    email: string,
    password: string
  ) => {
    const response = (await UserService.postApiUserRegister({
      requestBody: { userName: userName, password: password, email: email },
    })) as UserResponse

    return response
  }

  const changePassword = async (
    currentPassword: string,
    newPassword: string
  ) => {
    const response = (await UserService.postApiUserChangePassword({
      requestBody: {
        currentPassword: currentPassword,
        newPassword: newPassword,
      },
    })) as UserResponse

    if (response.success) {
      SessionStorage.set("password", newPassword)
    }

    return response
  }

  const logout = async () => {
    _userAuthorized.value = false
    OpenAPI.TOKEN = ""
    SessionStorage.clear()
  }

  return {
    userID,
    isAuthorized,
    tryAutoLogin,
    login,
    register,
    changePassword,
    logout,
  }
})
