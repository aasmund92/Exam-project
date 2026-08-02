export default async function formatInput(input: string, type: "name" | "email") {
    input = input.trim();
    console.log(input);
    
    if(input.length === 0) {
        return "invalid";
    };

    if(type === "name") {
       
       
        return input.charAt(0).toUpperCase() + input.slice(1).toLowerCase();
    };
    
    if(type === "email") {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const formattedEmail = input.toLowerCase().trim();

        if (!emailRegex.test(formattedEmail) || input.length === 0) {
            return "invalid";
        };
        return formattedEmail;
    };

    return input;
};