import { LocalStorage } from "quasar"
import { defineStore } from "pinia"
import type { User } from "firebase/auth"
import { FirebaseAuthorization } from "@/firebase/FirebaseAuthorization"
import { ref } from "vue"

export const useAuthStore = defineStore("auth", () => {
  const _authorization =
    LocalStorage.has("user") && LocalStorage.has("userAuthorized")
      ? ref(
          FirebaseAuthorization.ForUser(
            LocalStorage.getItem("user") as User,
            LocalStorage.getItem("userAuthorized") as boolean
          )
        )
      : ref(FirebaseAuthorization.NoUser())

  const userAuthorized = ref(_authorization.value.UserAuthorized())

  const login = async (email: string, password: string) => {
    const response = await _authorization.value.Login(email, password)
    userAuthorized.value = _authorization.value.UserAuthorized()
    return response
  }

  const signUp = async (email: string, password: string) => {
    const response = await _authorization.value.SignUp(email, password)
    userAuthorized.value = _authorization.value.UserAuthorized()
    return response
  }

  const logout = async () => {
    await _authorization.value.Logout()
  }

  return { userAuthorized, login, signUp, logout }
})
