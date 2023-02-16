import { LocalStorage } from "quasar"
import {
  createUserWithEmailAndPassword,
  getAuth,
  signInWithEmailAndPassword,
  signOut,
} from "firebase/auth"
import type { User } from "firebase/auth"
import { FirebaseError } from "@firebase/util"
import { FirebaseResponse } from "./FirebaseResponse"

export class FirebaseAuthorization {
  private readonly _auth = getAuth()
  private _user: User
  private _userAuthorized: boolean
  UserAuthorized = () => this._userAuthorized

  private constructor(user: User, userAuthorized: boolean) {
    this._user = user
    this._userAuthorized = userAuthorized
  }

  static ForUser(user: User, userAuthorized: boolean) {
    return new FirebaseAuthorization(user, userAuthorized)
  }

  static NoUser() {
    return new FirebaseAuthorization({} as User, false)
  }

  private SetUser(user: User, userAuthorized: boolean) {
    this._user = user
    this._userAuthorized = userAuthorized
    LocalStorage.set("user", user)
    LocalStorage.set("userAuthorized", userAuthorized)
  }

  SignUp = async (email: string, password: string) => {
    try {
      const response = await createUserWithEmailAndPassword(
        this._auth,
        email,
        password
      )
      if (response) {
        this.SetUser(response.user, true)
        return FirebaseResponse.Success()
      } else {
        return FirebaseResponse.Error("Sign up failed")
      }
    } catch (error) {
      let message =
        error instanceof FirebaseError ? error.message : JSON.stringify(error)
      return FirebaseResponse.Error(message)
    }
  }

  Login = async (email: string, password: string) => {
    try {
      const response = await signInWithEmailAndPassword(
        this._auth,
        email,
        password
      )
      if (response) {
        this.SetUser(response.user, true)
        return FirebaseResponse.Success()
      } else {
        return FirebaseResponse.Error("Login failed")
      }
    } catch (error) {
      let message =
        error instanceof FirebaseError ? error.message : JSON.stringify(error)
      return FirebaseResponse.Error(message)
    }
  }

  Logout = async () => {
    await signOut(this._auth)
    this.SetUser({} as User, false)
  }
}
