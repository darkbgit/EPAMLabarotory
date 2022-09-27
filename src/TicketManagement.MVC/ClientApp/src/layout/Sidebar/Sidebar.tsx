import { NavLink } from "react-router-dom";
import { SidebarProps } from "./Sidebar.props";
import styles from "./Sidebar.module.css";
import cn from "classnames";

export const Sidebar = ({ className, ...props }: SidebarProps): JSX.Element => {
  const setActive = ({ isActive }: any) =>
    isActive ? styles["menu-item-active"] : "";
  return (
    <div className={cn(className, styles.sidebar)} {...props}>
      <NavLink
        to="/edit-list"
        className={(cn(className, styles["menu-item"]), setActive)}
      >
        Edit Events
      </NavLink>
      <NavLink
        to="/third-party"
        className={(cn(className, styles["menu-item"]), setActive)}
      >
        Load Events from "Third party events" editor
      </NavLink>
    </div>
  );
};
