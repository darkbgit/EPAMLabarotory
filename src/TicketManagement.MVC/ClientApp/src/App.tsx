import React, { useEffect } from "react";
import "./App.css";
import { Navigate, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage";
import EventsModeratorPage from "./pages/EventsModeratorPage";
import { observer } from "mobx-react-lite";
import { Header } from "./layout/Header/Header";
import EventAreasModeratorPage from "./pages/EventAreasModeratorPage";
import EventSeatsModeratorPage from "./pages/EventSeatsModeratorPage";
import { ToastContainer } from "react-toastify";
import NotFound from "./pages/NotFoundPage";
import LoginForm from "./components/auth/LoginForm";
import { useStore } from "./stores/store";
import { Loading } from "./layout/Loading/Loading";
import ModalContainer from "./components/modals/ModalContainer";
import EventsMainPage from "./pages/EventsMainPage";
import EventDetailsMainPage from "./pages/EventDetailsMainPage";
import { Footer } from "./layout/Footer/Footer";
import BuyTicketsPage from "./pages/BuyTicketsPage";
import UserOrdersPage from "./pages/UserOrdersPage";

function App() {
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.setUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if (!commonStore.appLoaded) return <Loading />;

  return (
    <>
      <ToastContainer position="bottom-right" hideProgressBar />
      <ModalContainer />
      <Header />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route
          path="edit-list"
          element={
            <React.Suspense fallback={<div>Loading...</div>}>
              <EventsModeratorPage />
            </React.Suspense>
          }
        />
        <Route
          path="edit-list/:id/event-areas"
          element={<EventAreasModeratorPage />}
        />
        <Route
          path="edit-list/:eventId/event-areas/:id/event-seats"
          element={<EventSeatsModeratorPage />}
        />
        <Route path="main" element={<EventsMainPage />} />
        <Route path="main/:id" element={<EventDetailsMainPage />} />
        <Route path="main/:id/buy" element={<BuyTicketsPage />} />
        <Route path="orders/:id" element={<UserOrdersPage />} />
        <Route path="login" element={<LoginForm />} />
        <Route path="404" element={<NotFound />} />
        <Route path="*" element={<Navigate replace to="/404" />} />
      </Routes>
      <Footer />
    </>
  );
}

export default observer(App);
