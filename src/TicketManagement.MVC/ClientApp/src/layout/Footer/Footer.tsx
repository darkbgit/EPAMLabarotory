import { FooterProps } from "./Footer.props";
import styles from "./Footer.module.css";
import cn from "classnames";

export const Footer = ({ className, ...props }: FooterProps): JSX.Element => {
  const year = new Date().getFullYear();
  return (
    <footer className={cn(className, styles.footer)} {...props}>
      <div>TicketManagement @ 2022 - {year} All rights protected.</div>
    </footer>
  );
};
