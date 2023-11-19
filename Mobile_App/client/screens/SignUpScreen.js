import React, { useState } from "react";
import {
  Image,
  StatusBar,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import * as Icon from "react-native-feather";
import { useNavigation } from "@react-navigation/native";
import { SafeAreaView } from "react-native-safe-area-context";
import { themeColors } from "../theme";

export default function SignUpScreen() {
  const navigation = useNavigation();
  const [registrationSuccess, setRegistrationSuccess] = useState(false);
  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const handleSignUp = () => {
    // Thực hiện logic đăng ký ở đây
    // Để minh họa, tôi sẽ sử dụng setTimeout để giả lập đăng ký thành công
    setRegistrationSuccess(true);
    setTimeout(() => {
      setRegistrationSuccess(false);
      navigation.navigate("Login");
    }, 1000); // Chuyển hướng đến trang đăng nhập sau 2 giây
  };

  return (
    <View
      className="flex-1 bg-white"
      style={{ backgroundColor: themeColors.bg }}>
      <StatusBar
        backgroundColor={themeColors.bg}
        options={{ presentation: "fullScreenModal", headerShown: false }}
      />
      <SafeAreaView className="flex">
        <View className="flex-row justify-start">
          <TouchableOpacity
            style={{ backgroundColor: themeColors.bgColor(1) }}
            onPress={() => navigation.goBack()}
            className="absolute z-10 rounded-full p-2 shadow top-5 left-2">
            <Icon.ArrowLeft strokeWidth={3} stroke="white" />
          </TouchableOpacity>
        </View>
        <View className="flex-row justify-center">
          <Image
            source={require("../assets/images/signup.png")}
            style={{ width: 165, height: 110 }}
          />
        </View>
      </SafeAreaView>

      <View
        className="flex-1 bg-white px-8 pt-8"
        style={{ borderTopLeftRadius: 50, borderTopRightRadius: 50 }}>
        <View className="form space-y-2">
          <Text className="text-gray-700 ml-4">Họ và tên</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-7"
            value={formData.fullName}
            onChangeText={(text) =>
              setFormData({ ...formData, fullName: text })
            }
            placeholder="Họ và tên"
          />
          <Text className="text-gray-700 ml-4">Email</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-7"
            value={formData.email}
            onChangeText={(text) => setFormData({ ...formData, email: text })}
            placeholder="Email"
          />
          <Text className="text-gray-700 ml-4">Mật khẩu</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-7"
            value={formData.password}
            onChangeText={(text) =>
              setFormData({ ...formData, password: text })
            }
            secureTextEntry
            placeholder="Nhập mật khẩu"
          />
          <Text className="text-gray-700 ml-4">Nhập lại mật khẩu</Text>
          <TextInput
            className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-7"
            value={formData.confirmPasswor}
            onChangeText={(text) =>
              setFormData({ ...formData, confirmPasswor: text })
            }
            secureTextEntry
            placeholder="Nhập lại mật khẩu"
          />
          <TouchableOpacity
            onPress={handleSignUp}
            className="py-3 bg-yellow-400 rounded-xl">
            <Text className="font-xl font-bold text-center text-gray-700">
              Đăng ký
            </Text>
          </TouchableOpacity>
          <View className="flex-row justify-center mt-7">
            <Text className="text-gray-500 font-semibold">
              Đã có tài khoảng?
            </Text>
            <TouchableOpacity onPress={() => navigation.navigate("Login")}>
              <Text className="font-semibold text-yellow-500"> Đăng nhập</Text>
            </TouchableOpacity>
          </View>
          {registrationSuccess && (
            <View
              style={{
                position: "absolute",
                backgroundColor: "rgba(0, 0, 0, 0.8)",
                width: "100%",
                height: "100%",
                justifyContent: "center",
                alignItems: "center",
              }}>
              <Text style={{ color: "white", fontSize: 20 }}>
                Đăng ký thành công!
              </Text>
            </View>
          )}
        </View>
      </View>
    </View>
  );
}
