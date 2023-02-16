export class FirebaseResponse {
  Success: boolean
  Message: string

  private constructor(success: boolean, message: string) {
    this.Success = success
    this.Message = message
  }

  static Success() {
    return new FirebaseResponse(true, "")
  }

  static Error(message: string) {
    return new FirebaseResponse(false, message)
  }
}
