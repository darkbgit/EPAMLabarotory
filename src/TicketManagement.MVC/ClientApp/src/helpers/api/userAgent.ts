import {
  CreateUserModel,
  User,
  UserFormValues,
  UserInfo,
} from "../../interfaces/users";
import { createAxios, requests } from "./agentFactory";

const baseUrl = process.env.REACT_APP_USER_MANAGEMENT;

const userAgentInstance = createAxios(baseUrl!);

const Account = {
  current: () => requests.get<User>(userAgentInstance, "/users/user"),
  userInfo: (token: string) =>
    requests.post<UserInfo>(userAgentInstance, "/users/user-info", {
      token: token,
    }),
  login: (user: UserFormValues) =>
    requests.post<string>(userAgentInstance, "/auth/login", user),
  register: (user: CreateUserModel) =>
    requests.post<User>(userAgentInstance, "/users/register", user),
};

const userAgent = {
  Account,
};

export default userAgent;
