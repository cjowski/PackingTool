import { LocalStorage } from "quasar"
import { defineStore } from "pinia"
import { ref } from "vue"
import {
  OpenAPI,
  UserService,
  type AuthenticateResponse,
  type UserResponse,
} from "@/api"

export const useAuthenticationStore = defineStore("authentication", () => {
  const _userAuthorized = ref(false)

  const isAuthorized = () => _userAuthorized.value

  const login = async (userName: string, password: string) => {
    const response = (await UserService.postApiUserAuthenticate({
      requestBody: { userName: userName, password: password },
    })) as AuthenticateResponse

    if (response.success) {
      _userAuthorized.value = true
      OpenAPI.TOKEN = response.token!
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

  const logout = async () => {
    _userAuthorized.value = false
    OpenAPI.TOKEN = ""
  }

  return { isAuthorized, login, register, logout }
})
