import PersonIcon from '@mui/icons-material/Person';
import {useEffect, useState} from "react";
import keycloak from "../../../shared/api/KeyCloak.ts";
import {Button, Menu, MenuItem} from "@mui/material";

function Account() {
    const [isAuthorized, setIsAuthorized] = useState<boolean>(false);
    const [anchorEl, setAnchorEl] = useState<null|HTMLElement>(null);

    useEffect(() => {
        keycloak.init({
            onLoad: 'check-sso',
            pkceMethod: 'S256'
        }).then((auth : boolean) => {
            setIsAuthorized(auth);
        });
    }, []);

    const handleLogIn = () => {
        keycloak.login({
            redirectUri: import.meta.env.VITE_AUTH_SUCCESSFUL_REDIRECT_URL
        });
    }

    const handleLogOut = () => {
        keycloak.logout({
            redirectUri: import.meta.env.VITE_LOGOUT_REDIRECT_URL
        });
        setIsAuthorized(false);
    };

    const handleOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    return(
        <>
            {!isAuthorized? (
                <Button variant="contained" onClick={handleLogIn} sx = {{backgroundColor: "red", color: "white"}}>
                    Log in / Sign up
                </Button>
            ): (
                <>
                    <div onClick={handleOpen}>
                        <PersonIcon />
                    </div>
                    <Menu
                        open={Boolean(anchorEl)}
                        anchorEl={anchorEl}
                        onClose={() => {}}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'right',
                        }}
                    >
                        <MenuItem onClick={() => {}}>Profile</MenuItem>
                        <MenuItem onClick={handleLogOut} sx={{ color: 'red' }}>
                            Logout
                        </MenuItem>
                    </Menu>
                </>
            )}
        </>
    )
}

export default Account;
