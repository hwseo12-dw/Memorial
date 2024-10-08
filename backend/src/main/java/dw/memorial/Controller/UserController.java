package dw.memorial.Controller;

import dw.memorial.Dto.BaseResponse;
import dw.memorial.Dto.SessionDto;
import dw.memorial.Dto.UserDto;
import dw.memorial.Enumstatus.ResultCode;
import dw.memorial.Model.User;
import dw.memorial.Service.UserDetailService;
import dw.memorial.Service.UserService;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.context.HttpSessionSecurityContextRepository;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/user")
@CrossOrigin(origins = "http://localhost:3000")
public class UserController {
    private UserService userService;
    private UserDetailService userDetailService;
    private AuthenticationManager authenticationManager;
    private HttpServletRequest httpServletRequest;

    public UserController(UserService userService, UserDetailService userDetailService, AuthenticationManager authenticationManager, HttpServletRequest httpServletRequest) {
        this.userService = userService;
        this.userDetailService = userDetailService;
        this.authenticationManager = authenticationManager;
        this.httpServletRequest = httpServletRequest;
    }
    @GetMapping("show")
    public ResponseEntity<List<User>> getAllUsers(){
        return new ResponseEntity<>(userService.getAllUsers(),
                HttpStatus.OK);
    }

    @PostMapping("signup")
    public ResponseEntity<String> signup(@RequestBody UserDto userDto){
        return new ResponseEntity<>(userService.saveUser(userDto),
                HttpStatus.CREATED);
    }

//    @PostMapping("login")
//    public ResponseEntity<String> login(@RequestBody UserDto userDto,
//                                        HttpServletRequest request){
//        Authentication authentication = authenticationManager.authenticate(
//                new UsernamePasswordAuthenticationToken(userDto.getUserId(), userDto.getPassword())
//        );
//        SecurityContextHolder.getContext().setAuthentication(authentication);
//        // 세션 생성
//        HttpSession session = request.getSession(true); // true: 세션이 없으면 새로 생성
//        // 세션에 인증 객체 저장
//        session.setAttribute(HttpSessionSecurityContextRepository.SPRING_SECURITY_CONTEXT_KEY,
//                SecurityContextHolder.getContext());
//
//        return ResponseEntity.ok("Success");
//    }
    @PostMapping("login")
    public ResponseEntity<BaseResponse<Void>> login(@RequestBody UserDto userDto,
                                                    HttpServletRequest request){
        Authentication authentication = authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(userDto.getUserId(), userDto.getPassword())
        );
        SecurityContextHolder.getContext().setAuthentication(authentication);
        // 세션 생성
        HttpSession session = request.getSession(true); // true: 세션이 없으면 새로 생성
        // 세션에 인증 객체 저장
        session.setAttribute(HttpSessionSecurityContextRepository.SPRING_SECURITY_CONTEXT_KEY,
                SecurityContextHolder.getContext());

        return new ResponseEntity<>(
                new BaseResponse(ResultCode.SUCCESS.name(),
                        null,
                        ResultCode.SUCCESS.getMsg())
                , HttpStatus.OK);
    }

//    @PostMapping("logout")
//    public String logout(HttpServletRequest request, HttpServletResponse response){
//        HttpSession session = request.getSession(false);
//        if(session != null){
//            session.invalidate();
//        }
//        return "You have been logged out!!";
//    }

    @PostMapping("logout")
    public ResponseEntity<BaseResponse<String>> logout(HttpServletRequest request, HttpServletResponse response){
        HttpSession session = request.getSession(false);
        if(session != null){
            session.invalidate();
        }
        return new ResponseEntity<>(
                new BaseResponse<>(ResultCode.SUCCESS.name(),
                        "You have been logged out!",
                        ResultCode.SUCCESS.getMsg()),
                HttpStatus.OK);
    }

    @PatchMapping("/modify/{id}")
    public ResponseEntity<User> updateUser(@PathVariable String id,
            @RequestBody User user){
        return new ResponseEntity<>(userService.updateUser(id, user),
                HttpStatus.OK);
    }

//    @GetMapping("current")
//    public SessionDto getCurrentUser(){
//        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
//        if(authentication == null || !authentication.isAuthenticated()){
//            throw new IllegalStateException("User is not authenticated");
//        }
//        SessionDto sessionDto = new SessionDto();
//        sessionDto.setUserId(authentication.getName());
//        sessionDto.setAuthority(authentication.getAuthorities());
//        return sessionDto;
//    }

    @GetMapping("current")
    public ResponseEntity<BaseResponse<SessionDto>> getCurrentUser(){
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if(authentication == null || !authentication.isAuthenticated()) {
            throw new IllegalStateException("User is not authenticated!");
        }
        SessionDto sessionDto = new SessionDto();
        sessionDto.setUserId(authentication.getName());
        sessionDto.setAuthority(authentication.getAuthorities());

        return new ResponseEntity<>(
                new BaseResponse(ResultCode.SUCCESS.name(),
                        sessionDto,
                        ResultCode.SUCCESS.getMsg())
                , HttpStatus.OK);
    }

    // 대시보드 사용자 관리를 위한 유저 정보 불러오기
    @GetMapping("getallusersparts")
    public ResponseEntity<List<Object[]>> getAllUsersParts(){
        return new ResponseEntity<>(userService.getAllUsersParts(),
                HttpStatus.OK);
    }

    // 유저 검색
    @GetMapping("showsomeone/{id}")
    public ResponseEntity<List<User>> showSomeone(@PathVariable String id){
        return new ResponseEntity<>(userService.showSomeone(id),
                HttpStatus.OK);
    }



}
