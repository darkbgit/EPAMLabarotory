export interface User {
  id: string;
  timeZoneId: string;
  language: string;
  userName: string;
  firstName: string;
  surname: string;
  balance: number;
  phoneNumber: string;
  email: string;
}

export interface UserWithLocale {
  id: string;
  timeZoneId: string;
  language: string;
  userName: string;
  firstName: string;
  surname: string;
  balance: number;
  phoneNumber: string;
  email: string;
  locale: Locale;
  roles: string[];
}

export interface UserInfo {
  id: string;
  locale: string;
  timeZoneId: string;
  roles: string[];
}

export interface UserFormValues {
  userNameOrEmail: string;
  password: string;
  displayName?: string;
  userName?: string;
}

export interface CreateUserModel {
  userName: string;
  email: string;
  password: string;
}
