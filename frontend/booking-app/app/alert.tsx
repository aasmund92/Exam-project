import Alert from "@mui/material/Alert";

interface CustomAlertProps {
  message: string | null;
  severity?: "success" | "error";
  onClose: () => void;
};

export default function CustomAlert({ message, severity = "success", onClose }: CustomAlertProps) {
  if (!message) return null; 

  return (
    <div className="max-w-sm mx-auto mb-4">
      <Alert severity={severity} onClose={onClose} sx={{ width: "100%" }}>
        {message}
      </Alert>
    </div>
  );
};