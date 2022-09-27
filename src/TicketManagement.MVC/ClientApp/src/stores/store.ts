import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import EventPurchaseStore from "./eventPurchaseStore";
import EventStore from "./eventStore";
import ModalStore from "./modalStore";
import UserStore from "./userStore";

interface Store {
  eventStore: EventStore;
  userStore: UserStore;
  commonStore: CommonStore;
  modalStore: ModalStore;
  eventPurchaseStore: EventPurchaseStore;
}

export const store: Store = {
  eventStore: new EventStore(),
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  modalStore: new ModalStore(),
  eventPurchaseStore: new EventPurchaseStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
