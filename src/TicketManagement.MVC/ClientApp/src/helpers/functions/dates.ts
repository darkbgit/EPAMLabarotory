export const getDateWithoutTimezoneOffset = (date: Date): number => {
  const millisecondsInHour = 60000;
  return date.getTime() - date.getTimezoneOffset() * millisecondsInHour;
};
