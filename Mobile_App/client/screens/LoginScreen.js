import React, { useState } from "react";
import {
  Image,
  StatusBar,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { useNavigation } from "@react-navigation/native";
import * as Icon from "react-native-feather";
import { SafeAreaView } from "react-native-safe-area-context";
import { themeColors } from "../theme";

export default function LoginScreen() {
  const navigation = useNavigation();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = () => {
    if (email !== "" && password !== "") {
      // Xử lý đăng nhập thành công, chuyển hướng đến trang chủ
      navigation.navigate("Home"); // Thay 'Home' bằng tên màn hình trang chủ của bạn
    } else {
      // Hiển thị thông báo lỗi hoặc xử lý đăng nhập không thành công
      // Ví dụ: Alert.alert('Thông báo', 'Vui lòng nhập đầy đủ thông tin đăng nhập.');
    }
  };

  return (
    <View
      className="flex-1 bg-white"
      style={{ backgroundColor: themeColors.bg }}>
      <StatusBar
        backgroundColor={themeColors.bg}
        options={{ presentation: "fullScreenModal", headerShown: false }}
      />
      <SafeAreaView className="flex ">
        <View className="flex-row justify-start">
          <TouchableOpacity
            style={{ backgroundColor: themeColors.bgColor(1) }}
            onPress={() => navigation.goBack()}
            className="absolute z-10 rounded-full p-2 shadow top-5 left-2">
            <Icon.ArrowLeft strokeWidth={3} stroke="white" />
          </TouchableOpacity>
        </View>
        <View className="flex-row justify-center" style={{ height: 220 }}>
          <Image
            source={require("../assets/images/login.png")}
            style={{ width: 200, height: 200 }}
          />
        </View>
      </SafeAreaView>
      <View
        style={{ borderTopLeftRadius: 50, borderTopRightRadius: 50 }}
        className="flex-1 bg-white px-8 pt-8">
        <View className="form space-y-2">
          <Text className="text-gray-700 ml-4">Email</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-3"
            placeholder="Email@"
            value={email}
            onChangeText={(text) => setEmail(text)}
          />
          <Text className="text-gray-700 ml-4">Mật khẩu</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl"
            secureTextEntry
            placeholder="Mật khẩu"
            value={password}
            onChangeText={(text) => setPassword(text)}
          />
          <TouchableOpacity
            onPress={() => console.log("Forgot password clicked")}
            className="flex items-end">
            <Text className="text-gray-700 mb-5">Quên mật khẩu?</Text>
          </TouchableOpacity>

          <TouchableOpacity
            className="py-3 bg-yellow-400 rounded-xl"
            onPress={handleLogin}>
            <Text className="text-xl font-bold text-center text-gray-700">
              Đăng nhập
            </Text>
          </TouchableOpacity>
        </View>
        <View className="flex-row mt-5 justify-center space-x-12">
          <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
            <Image
              source={require("../assets/icons/google.png")}
              className="w-10 h-10"
            />
          </TouchableOpacity>
          <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
            <Image
              source={require("../assets/icons/gmail.png")}
              className="w-10 h-10"
            />
          </TouchableOpacity>
          <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
            <Image
              source={require("../assets/icons/facebook.png")}
              className="w-10 h-10"
            />
          </TouchableOpacity>
        </View>
        <View className="flex-row justify-center mt-5">
          <Text className="text-gray-500 font-semibold">
            Không có tài khoản?
          </Text>
          <TouchableOpacity onPress={() => navigation.navigate("SignUp")}>
            <Text className="font-semibold text-yellow-500 ml-2">Đăng ký</Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
}
