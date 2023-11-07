import { useNavigation } from '@react-navigation/native'
import React from 'react'
import { Image, StatusBar, Text, TextInput, TouchableOpacity, View } from 'react-native'
import * as Icon from "react-native-feather"
import { SafeAreaView } from 'react-native-safe-area-context'
import { themeColors } from '../theme'


export default function LoginScreen() {
  const navigation = useNavigation();
  return (
     <View className="flex-1 bg-white" style={{backgroundColor: themeColors.bg}}>
      <StatusBar
        backgroundColor={themeColors.bg}
        options={{ presentation: 'fullScreenModal', headerShown: false}}
      />
      <SafeAreaView  className="flex ">
        <View className="flex-row justify-start">
        <TouchableOpacity 
            style={{backgroundColor: themeColors.bgColor(1)}} 
            onPress={()=> navigation.goBack()} 
            className="absolute z-10 rounded-full p-2 shadow top-5 left-2">
            <Icon.ArrowLeft strokeWidth={3} stroke="white" />
        </TouchableOpacity>
        </View>
        <View className="flex-row justify-center" style={{height:220}}>
          <Image source={require('../assets/images/login.png')} 
          style={{width: 200, height: 200}} />
        </View>
        
        
      </SafeAreaView>
      <View 
        style={{borderTopLeftRadius: 50, borderTopRightRadius: 50}} 
        className="flex-1 bg-white px-8 pt-8">
          <View className="form space-y-2">
            <Text className="text-gray-700 ml-4">Email</Text>
            <TextInput 
              className="p-4 bg-gray-100 text-gray-700 rounded-2xl mb-3"
              placeholder="email"
              value="" 
            />
            <Text className="text-gray-700 ml-4">Mật khẩu</Text>
            <TextInput 
              className="p-4 bg-gray-100 text-gray-700 rounded-2xl"
              secureTextEntry
              placeholder="password"
              value="" 
            />
            <TouchableOpacity className="flex items-end">
              <Text className="text-gray-700 mb-5">Quên mật khẩu?</Text>
            </TouchableOpacity>
            <TouchableOpacity 
              className="py-3 bg-yellow-400 rounded-xl">
                
                <Text 
                    className="text-xl font-bold text-center text-gray-700"
                >
                        Đăng nhập
                </Text>
             </TouchableOpacity>
            
          </View>
          <Text className="text-xl text-gray-700 font-bold text-center py-5">Or</Text>
          <View className="flex-row justify-center space-x-12">
            <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
              <Image source={require('../assets/icons/google.png')} className="w-10 h-10" />
            </TouchableOpacity>
            <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
              <Image source={require('../assets/icons/gmail.png')} className="w-10 h-10" />
            </TouchableOpacity>
            <TouchableOpacity className="p-2 bg-gray-100 rounded-2xl">
              <Image source={require('../assets/icons/facebook.png')} className="w-10 h-10" />
            </TouchableOpacity>
          </View>
          <View className="flex-row justify-center mt-7">
              <Text className="text-gray-500 font-semibold">
                  Không có tài khoản?
              </Text>
              <TouchableOpacity onPress={()=> navigation.navigate('SignUp')}>
                  <Text className="font-semibold text-yellow-500"> Đăng ký</Text>
              </TouchableOpacity>
          </View>
          
      </View>
    </View>
  )
}